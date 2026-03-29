

using System.Collections.Generic;
using UnityEngine;

public class BuildingDataTable : MonoBehaviour
{
    public static BuildingDataTable Instance { get; private set; }
    public Dictionary<string, string> buildingData = new Dictionary<string, string>();

    private void Awake()
    {
        Debug.Log("🔹 [BuildingDataTable] Awake 开始");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("✅ Instance 已赋值");
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeData();

        Debug.Log($"🎉 数据加载完成！共 {buildingData.Count} 条");
    }

    void InitializeData()
    {
        buildingData.Clear();
        Debug.Log("📦 开始初始化数据...");

        buildingData.Add("下昂", "一种利用重心转移与斜面嵌合原理的榫卯结构，受力时越压越紧，稳固牢靠，动中取固，工艺精妙。");
        buildingData.Add("交互斗", "斗拱系统中多个斗与拱在空间中相互咬合、层层递进，形成稳固的立体网络，兼具承重与抗震之效。");
        buildingData.Add("令拱", "斗拱结构中斜向上挑的过渡构件，承托上部重量，令屋檐外展挺拔，刚柔相济，力学与美感并存。");
        buildingData.Add("内槽柱头半驼峰", "柱头与斗拱之间的连接构件，常带半驼峰与内槽设计，既缓冲传力又具装饰性，结构美学融合之作。");
        buildingData.Add("内槽补间一跳华拱", "内檐补间铺作第一跳出拱，承托梁架，传递荷载，层次分明，体现宋代营造法式之精妙。");
        buildingData.Add("内槽补间三跳华拱", "内檐补间铺作第三跳出拱，层层递进，出跳深远，增强结构稳定性，彰显斗拱体系之复杂精妙。");
        buildingData.Add("内槽补间二跳华拱", "内檐补间铺作第二跳出拱，承上启下，连接内外，力学传递流畅，结构层次清晰。");
        buildingData.Add("内槽补间半驼峰", "内檐补间处的半驼峰构件，用于过渡连接，缓冲应力集中，兼具结构功能与装饰美感。");
        buildingData.Add("卢拱", "因形似古代盛粮器卢而得名，常位于斗拱下部，承托拱臂、分散荷载，结构坚实且富于美感。");
        buildingData.Add("压槽枋", "位于斗拱上部的横向构件，用于压紧槽口、固定榫卯，并承托上层梁架，是斗拱与屋架间的纽带。");
        buildingData.Add("四元明", "斗拱结构中在四个方向各出一拱，形成对称均衡的十字布局，是典型的单杪斗拱构造形式之一。");
        buildingData.Add("门内柱", "位于门洞内侧的立柱，支撑门框结构，稳定门扇开合，是建筑出入口的重要承重构件。");
        buildingData.Add("地付", "柱子底部与基座之间的过渡构件，用于稳定柱脚，分散垂直荷载，防潮防腐，延长柱体寿命。");
        buildingData.Add("垫木", "置于柱下或梁上，用以调平、减震或加固的木质构件，缓冲应力，保护主体结构。");
        buildingData.Add("外檐柱头", "位于檐部柱顶端，与斗拱和梁架相连接的部位，承托屋檐重量，是内外结构转换的关键节点。");
        buildingData.Add("外檐柱间跳华拱", "外檐柱间设置的跳华拱，向外挑出，支撑檐部，增强出檐深度，兼具结构承重与立面装饰功能。");
        buildingData.Add("外檐补间跳华拱", "外檐补间铺作中的跳华拱，填充柱间空隙，增强整体刚度，使斗拱体系更加完整稳固。");
        buildingData.Add("外檐补间耍头", "外檐补间铺作末端的耍头构件，形如鸟喙，既固定拱端，又具装饰效果，是斗拱收束之点睛笔。");
        buildingData.Add("左大门", "建筑左侧主入口门扇，通常与右大门对称设置，体现中国传统建筑的中轴对称美学。");
        buildingData.Add("右大门", "建筑右侧主入口门扇，与左大门成对，共同构成建筑主入口，象征庄重与礼仪。");
        buildingData.Add("平棋枋", "水平横枋，起装饰与结构稳定作用，常用于梁与梁之间，均衡受力，美化空间层次。");
        buildingData.Add("底座", "建筑构件的基础支撑部分，承载上部全部重量，分散荷载至地基，是结构稳固之根本。");
        buildingData.Add("慢拱", "斜向伸出的拱臂，出跳较缓，连接斗与拱，支撑上部构件，节奏舒缓，力学传递平稳。");
        buildingData.Add("散斗", "散布设置的小斗，用于承托上部拱臂，增强结构层次，分散局部应力，是斗拱体系的基本单元。");
        buildingData.Add("斜项", "斗拱系统中斜向上挑的构件，用于支撑挑檐或过渡结构，角度精准，力学巧妙。");
        buildingData.Add("明乳付", "斗拱中可见的挑出小拱，造型如乳头，具有装饰和结构作用，细腻精巧，体现工匠智慧。");
        buildingData.Add("替木", "用于承受柱子传来的压力并分布荷载的枕木，常设于柱础上，缓冲冲击，保护柱脚。");
        buildingData.Add("柱头跳华拱", "柱头上第一跳出栱拱，通常装饰华丽，承托上部梁架，是斗拱体系的核心承重构件。");
        buildingData.Add("柱头枋", "位于柱头位置的横向枋木，连接相邻柱头，增强整体刚度，传递水平荷载，稳定框架结构。");
        buildingData.Add("拱眼壁", "斗拱之间填充的小型墙面，用于美化结构并遮挡缝隙，兼具装饰与围护功能，细腻精致。");
        buildingData.Add("拱眼壁外", "位于斗拱外侧的拱眼壁构件，封闭拱间空隙，防风防尘，同时丰富立面层次与视觉效果。");
        buildingData.Add("椽子", "铺设在檩条上，用以支撑屋面板瓦的细长圆木，排列整齐，构成屋面骨架，是屋顶结构的基础。");
        buildingData.Add("檐柱", "位于屋檐边缘的柱子，支撑檐部结构和斗拱，承受屋顶外挑重量，是建筑外廓的主要承重构件。");
        buildingData.Add("泥道拱", "位于内檐的拱形构件，多用于承托横梁或装饰用途，曲线优美，结构功能与艺术表现兼备。");
        buildingData.Add("燕尾", "屋脊两端向上翘起如燕尾状的装饰构件，兼具排水与装饰功能，寓意吉祥，是传统建筑的重要标志。");
        buildingData.Add("牛脊枋", "位于屋顶脊部的木构件，用于固定脊瓦，形似牛背，承托屋脊重量，稳定屋面结构。");
        buildingData.Add("瓜拱", "形如瓜瓣的栱拱构件，常用于拱眼中作装饰用，曲线柔和，寓意丰收吉祥，结构美学融合。");
        buildingData.Add("瓦片", "铺设于屋面的陶制或琉璃覆盖材料，层层叠压，防水防晒，保护木结构，是建筑外衣。");
        buildingData.Add("素枋", "无雕饰的横枋，用于简洁区域，起结构支撑作用，质朴无华，体现大巧若拙的营造理念。");
        buildingData.Add("牌匾", "悬挂于门楣或梁枋上的题字木板，标明建筑名称或寓意，兼具标识功能与文化表达。");
        buildingData.Add("罗汉枋", "上部略凸呈罗汉头状的横枋，常用于内檐美化结构，层次分明，富有节奏感与装饰性。");
        buildingData.Add("翼形拱", "斗拱中外挑、形似鸟翼的拱臂，兼具承重与美观，出檐深远，轻盈灵动，展现木构建筑之精妙。");
        buildingData.Add("草乳枋华拱头", "草乳枋末端的华拱构件，连接梁枋与斗拱，过渡自然，受力合理，体现榫卯结构之智慧。");
        buildingData.Add("覆莲柱础", "柱础形式之一，形似倒置的莲花，为柱脚提供美观和稳固基础，寓意清净庄严，防潮防腐。");
        buildingData.Add("门墩下", "位于门柱下方的石质或木质墩台，支撑门框，稳定门扇，防止沉降，是门构的基础构件。");
        buildingData.Add("门颊", "门框两侧的垂直木板或石块，用于固定门扇并增强门洞结构，稳固牢靠，界定空间边界。");
        buildingData.Add("阑额", "设于斗拱与柱顶之间的横木，常雕饰华丽，兼具承重与装饰，连接柱网，增强整体性。");
        buildingData.Add("齐心斗", "四方等距设置、重心统一的小斗，多见于平衡受力设计中，对称均衡，体现中庸之道。");
        buildingData.Add("缴背", "位于梁架背后的支撑构件，防止梁体下垂或弯曲，增强结构整体性，是木构架的重要加固措施。");
        buildingData.Add("门额", "门框上方的横向构件，也称门楣，承托门框上部重量，稳定门洞结构，常雕饰精美，兼具承重与装饰功能。");
        buildingData.Add("门槛", "门下横木或石条，用于分隔内外空间、稳定门框结构，兼具防风防尘功能，是门构的重要组成部分。");
        // 宝鼎园相关
        buildingData.Add("宝鼎园", "以宝鼎为核心的园林建筑群，融合祭祀、观赏与礼仪功能，体现古代'鼎'文化的庄重与神圣。");
        buildingData.Add("围栏", "围绕建筑或庭院的防护构件，既界定空间范围，又具装饰美化作用，常雕刻精美纹饰。");
        buildingData.Add("基座", "建筑构件的底部支撑平台，承载上部全部重量，分散荷载至地基，是结构稳固之根本。");
        buildingData.Add("石球柱头", "柱顶装饰构件，以石球造型收束柱体，兼具稳固柱头与美化视觉效果，寓意圆满吉祥。");

        // 翘角
        buildingData.Add("翘角", "屋檐四角向上翘起的飞檐构件，轻盈灵动，利于排水采光，是中国古建筑最具标志性的美学特征。");

        // 滴水瓦
        buildingData.Add("滴水瓦", "铺设于屋檐边缘的特殊瓦件，前端下垂呈滴水状，引导雨水远离墙体，保护木结构免受侵蚀。");

        // 挂落
        buildingData.Add("挂落", "悬挂于梁枋下方的装饰构件，镂空雕刻，层次丰富，既美化室内空间，又体现工匠精湛技艺。");

        // 戗兽
        buildingData.Add("戗兽", "位于屋脊戗角处的神兽装饰，镇宅辟邪，等级象征，数量与种类反映建筑规格与主人身份。");

        // 花格窗
        buildingData.Add("花格窗", "窗棂采用几何或花卉图案的镂空窗扇，通风采光，隔而不断，兼具实用功能与装饰美学。");

        // 檩条
        buildingData.Add("檩条", "横向架设于梁架之上的承重构件，支撑椽子与屋面，传递荷载至立柱，是屋顶结构的主骨架。");

        // 宝塔脊饰
        buildingData.Add("宝塔脊饰", "屋脊中央的塔形装饰构件，造型挺拔，寓意吉祥，增强建筑垂直感与宗教庄严氛围。");

        // 立柱
        buildingData.Add("立柱", "垂直支撑建筑主体的核心构件，承载梁架与屋顶重量，传递荷载至基础，是木构架的'骨骼'。");

        // 封檐板
        buildingData.Add("环形封檐板", "围绕屋檐边缘的环形封闭构件，遮挡椽头与檩条端部，防风防尘，美化屋檐立面。");
        buildingData.Add("异形封檐板", "根据屋檐造型定制的非标准封檐板，适应曲线或折角屋面，既实用又富于变化。");

        // 攒尖顶
        buildingData.Add("攒尖顶", "屋顶形式之一，四面或多面坡向中心汇聚于一点，形成尖顶，常用于亭台楼阁，造型优美。");

        // 墙体
        buildingData.Add("墙体", "建筑的围护结构，分隔内外空间，保温隔热，承重或填充，是建筑的基本组成部分。");

        // 石质围墙
        buildingData.Add("石质围墙", "以石材砌筑的围护墙体，坚固耐久，防御防护，界定领域范围，常雕刻装饰纹样。");
        buildingData.Add("小青瓦", "传统屋面铺设的弧形陶瓦，尺寸较小，层层叠压，防水防晒，色泽青灰，古朴典雅，是中国古建筑最具代表性的屋面材料之一。");
    }

    
    public int GetDataCount() => buildingData.Count;

    /// <summary>
    /// 智能查找构件描述（支持多种匹配方式）
    /// </summary>
    public string GetDescription(string buildingName)
    {
        if (string.IsNullOrEmpty(buildingName))
            return "名称为空";

        // 1. 精确匹配
        if (buildingData.TryGetValue(buildingName, out string description))
        {
            return description;
        }

        // 2. 去掉问号后匹配（处理"散斗?" → "散斗"）
        string cleanName = buildingName.Replace("?", "").Trim();
        if (buildingData.TryGetValue(cleanName, out description))
        {
            Debug.Log($"🔧 自动修正: [{buildingName}] → [{cleanName}]");
            return description;
        }

        // 3. 同音字/形近字替换（拱↔栱）
        string normalized = NormalizeBuildingName(buildingName);
        if (buildingData.TryGetValue(normalized, out description))
        {
            Debug.Log($"🔧 字符标准化: [{buildingName}] → [{normalized}]");
            return description;
        }

        // 4. 包含匹配（检查字典key是否包含构件名）
        foreach (var kvp in buildingData)
        {
            if (kvp.Key.Contains(cleanName) || cleanName.Contains(kvp.Key))
            {
                Debug.Log($"🔧 包含匹配: [{buildingName}] → [{kvp.Key}]");
                return kvp.Value;
            }
        }

        return $"未找到描述: {buildingName}";
    }

    /// <summary>
    /// 标准化构件名称（处理常见异体字）
    /// </summary>
    private string NormalizeBuildingName(string name)
    {
        // 拱 ↔ 栱
        name = name.Replace("栱", "拱");

        // 枋 ↔ 方
        // name = name.Replace("方", "枋");

        // 斗 ↔ 鬥（如果需要）
        // name = name.Replace("鬥", "斗");

        return name;
    }

    public bool ContainsBuilding(string buildingName)
    {
        return buildingData.ContainsKey(buildingName);
    }

}   