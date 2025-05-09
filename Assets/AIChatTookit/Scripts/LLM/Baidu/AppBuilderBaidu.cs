using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AppBuilderBaidu : LLM
{

	#region Params

	/// <summary>
	/// app id
	/// </summary>
	[SerializeField] private string app_id = string.Empty;
	/// <summary>
	/// api key
	/// </summary>
	[SerializeField] private string api_key = string.Empty;
	/// <summary>
	/// 新建会话API地址
	/// </summary>
	private string m_ConversationUrl=string.Empty;
	/// <summary>
	/// 对话ID
	/// </summary>
	[SerializeField] private string m_ConversationID = string.Empty;

	#endregion

	#region Public Method
	/// <summary>
	/// 发送消息
	/// </summary>
	/// <returns></returns>
	public override void PostMsg(string _msg, Action<string> _callback)
	{
		//缓存发送的信息列表
		m_DataList.Add(new SendData("user", _msg));
		StartCoroutine(Request(_msg, _callback));
	}


	/// <summary>
	/// 发送数据
	/// </summary> 
	/// <param name="_postWord"></param>
	/// <param name="_callback"></param>
	/// <returns></returns>
	public override IEnumerator Request(string _postWord, System.Action<string> _callback)
	{
		stopwatch.Restart();
		string jsonPayload = JsonConvert.SerializeObject(new RequestData
		{
			app_id= app_id,
			query = _postWord,
			conversation_id = m_ConversationID
		});

		using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
		{
			byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
			request.uploadHandler = (UploadHandler)new UploadHandlerRaw(data);
			request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

			request.SetRequestHeader("Content-Type", "application/json");
			request.SetRequestHeader("X-Appbuilder-Authorization", string.Format("Bearer {0}", api_key));

			yield return request.SendWebRequest();

			if (request.responseCode == 200)
			{
				string _msg = request.downloadHandler.text;
				ResponseData response = JsonConvert.DeserializeObject<ResponseData>(_msg);

				if (response.code == string.Empty)
				{
					string _msgBack = response.answer;
					//添加记录
					m_DataList.Add(new SendData("assistant", _msgBack));
					//回调
					_callback(_msgBack);
				}
				else
				{
					Debug.LogError(response.message);
				}
			}
			else
			{
				Debug.Log(request.error);
			}

		}

		stopwatch.Stop();
		Debug.Log("BaiduAppBuilder回复耗时：" + stopwatch.Elapsed.TotalSeconds);
	}

	#endregion



	#region Private Method

	void Awake()
	{
		OnInitial();
	}


	/// <summary>
	/// 初始化
	/// </summary>
	private void OnInitial()
	{

		//新建会话地址
		m_ConversationUrl = "https://qianfan.baidubce.com/v2/app/conversation";

		//聊天api地址
		url = "https://qianfan.baidubce.com/v2/app/conversation/runs";

		//新建会话
		StartCoroutine(OnStartConversation());

	}

	/// <summary>
	/// 新建会话
	/// </summary>
	/// <returns></returns>
	private IEnumerator OnStartConversation()
	{
		string jsonPayload = JsonUtility.ToJson(new CreateConversationData { app_id = app_id });
		using (UnityWebRequest request = new UnityWebRequest(m_ConversationUrl, "POST"))
		{
			byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
			request.uploadHandler = (UploadHandler)new UploadHandlerRaw(data);
			request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

			request.SetRequestHeader("Content-Type", "application/json");
			request.SetRequestHeader("X-Appbuilder-Authorization", string.Format("Bearer {0}", api_key));

			yield return request.SendWebRequest();

			if (request.responseCode == 200)
			{
				string _msg = request.downloadHandler.text;
				ConversationCreateReponse response = JsonConvert.DeserializeObject<ConversationCreateReponse>(_msg);

				if (response.code == string.Empty)
				{
					//获取到会话ID
					m_ConversationID = response.conversation_id;
				}
				else
				{
					Debug.LogError(response.message);
				}
			}
			else
			{
				Debug.Log(request.error);
			}

		}



	}



	#endregion


	#region Data Define

	/// <summary>
	/// 新建会话
	/// </summary>
	[Serializable]
	public class CreateConversationData
	{
		public string app_id=string.Empty;
	}
	[Serializable]
	public class ConversationCreateReponse
	{
		public string request_id = string.Empty;
		public string conversation_id = string.Empty;
		public string code = string.Empty;
		public string message = string.Empty;
	}


	/// <summary>
	/// 发送数据
	/// </summary>
	[Serializable]
	public class RequestData
	{
		public string app_id = string.Empty;//appID
		public string query = string.Empty;//提问内容
		public bool stream = false;//是否流式回答-本例不适用流式
		public string conversation_id = string.Empty;//对话ID
		public List<string> file_ids=new List<string>();//如果在对话中上传了文件，可以将文件id放入该字段，目前只处理第一个文件
	}


	[Serializable]
	public class ResponseData
	{
		public string code = string.Empty;//错误码
		public string message = string.Empty;//错误信息
		public string request_id = string.Empty;//request_id便于追踪。
		public string date = string.Empty;//消息返回时间的时间戳 UTC时间格式。
		public string answer = string.Empty;//文字答案。 流式场景下是增量数据。
		public string conversation_id = string.Empty;//对话ID
		public string message_id = string.Empty;//消息id, 流式场景下多次推流message_id保持一致。
		//public bool is_completion = false;//流式消息推送回答结果是否完结。
		//content 暂不定义，需要的话自己拓展


	}

	#endregion


}
