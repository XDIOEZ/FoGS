using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDataTable : MonoBehaviour
{
    public static Dictionary<string,string> buildingData = new Dictionary<string, string>();
    void Start()
    {
        buildingData.Add("下昂一","一种利用重心转移与斜面嵌合原理的榫卯结构，受力时越压越紧，稳固牢靠，动中取固，工艺精妙。");
        buildingData.Add("交互斗","斗拱系统中多个斗与拱在空间中相互咬合、层层递进，形成稳固的立体网络，兼具承重与抗震之效。");
        buildingData.Add("令拱","斗拱结构中斜向上挑的过渡构件，承托上部重量，令屋檐外展挺拔，刚柔相济，力学与美感并存。");
        buildingData.Add("平房","柱头与斗拱之间的连接构件，常带半驼峰与内槽设计，既缓冲传力又具装饰性，结构美学融合之作。");
        buildingData.Add("卢斗","因形似古代盛粮器“卢”而得名，常位于斗拱下部，承托拱臂、分散荷载，结构坚实且富于美感。");
        buildingData.Add("压槽枋","位于斗拱上部的横向构件，用于压紧槽口、固定榫卯，并承托上层梁架，是斗拱与屋架间的纽带。");
        buildingData.Add("四元明","斗拱结构中在四个方向各出一拱，形成对称均衡的十字布局，是典型的单杪斗拱构造形式之一。");
        buildingData.Add("地付", "柱子底部与基座之间的过渡构件，用于稳定柱脚。");
        buildingData.Add("圆柱", "常见的柱形构件，柱身圆形，多用于支撑屋顶重量。");
        buildingData.Add("垫木", "置于柱下或梁上，用以调平、减震或加固的木质构件。");
        buildingData.Add("外檐柱头", "位于檐部柱顶端，与斗拱和梁架相连接的部位。");
        buildingData.Add("平棋枋", "水平横枋，起装饰与结构稳定作用，常用于梁与梁之间。");
        buildingData.Add("慢拱", "斜向伸出的拱臂，出跳较缓，连接斗与拱，支撑上部构件。");
        buildingData.Add("散斗", "散布设置的小斗，用于承托上部拱臂，增强结构层次。");
        buildingData.Add("斜项", "斗拱系统中斜向上挑的构件，用于支撑挑檐或过渡结构。");
        buildingData.Add("明乳付", "斗拱中可见的挑出小拱，造型如乳头，具有装饰和结构作用。");
        buildingData.Add("替木", "用于承受柱子传来的压力并分布荷载的枕木，常设于柱础上。");
        buildingData.Add("柱头一跳华栱", "柱头上第一跳出栱拱，通常装饰华丽，承托上部梁架。");
        buildingData.Add("椽子", "铺设在檩条上，用以支撑屋面板瓦的细长圆木。");
        buildingData.Add("檐柱", "位于屋檐边缘的柱子，支撑檐部结构和斗拱。");
        buildingData.Add("泥道栱", "位于内檐的拱形构件，多用于承托横梁或装饰用途。");
        buildingData.Add("栱眼壁", "斗拱之间填充的小型墙面，用于美化结构并遮挡缝隙。");
        buildingData.Add("燕尾", "屋脊两端向上翘起如燕尾状的装饰构件，兼具排水与装饰功能。");
        buildingData.Add("牛脊枋", "位于屋顶脊部的木构件，用于固定脊瓦，形似牛背。");
        buildingData.Add("瓜栱", "形如瓜瓣的栱拱构件，常用于拱眼中作装饰用。");
        buildingData.Add("素枋", "无雕饰的横枋，用于简洁区域，起结构支撑作用。");
        buildingData.Add("罗汉枋", "上部略凸呈“罗汉头”状的横枋，常用于内檐美化结构。");
        buildingData.Add("翼形栱", "斗拱中外挑、形似鸟翼的拱臂，兼具承重与美观。");
        buildingData.Add("缴背", "位于梁架背后的支撑构件，防止梁体下垂或弯曲。");
        buildingData.Add("覆莲柱础", "柱础形式之一，形似倒置的莲花，为柱脚提供美观和稳固基础。");
        buildingData.Add("门墩", "位于门柱下方、起支撑与装饰作用的石质构件。");
        buildingData.Add("门槛", "门下横木或石条，用于分隔内外空间、稳定门框结构。");
        buildingData.Add("门颊", "门框两侧的垂直木板或石块，用于固定门扇并增强门洞结构。");
        buildingData.Add("阑额", "设于斗拱与柱顶之间的横木，常雕饰华丽，兼具承重与装饰。");
        buildingData.Add("齐心斗", "四方等距设置、重心统一的小斗，多见于平衡受力设计中。");

    }
}
