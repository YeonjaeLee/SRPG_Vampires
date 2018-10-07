using UnityEngine;
using System;
//using Excel = Microsoft.Office.Interop.Excel;
//using System.Runtime.InteropServices;
using System.Collections.Generic;
using Info;
using System.Xml;

public class ReadMap : MonoBehaviour
{

    void Start()
    {
        //ReadExcelData(@"C:\Diamondgames\project\project\SRPG_1\Assets\Resources\Map");
        LoadXML(xmlFileName);
    }

    // Resources/Map1/MapInfo.XML 파일.
    public string xmlFilePath = "Map1";
    string xmlFileName = "MapInfo";
    
    public void LoadXML(string _fileName)
    {
        TextAsset txtAsset = (TextAsset)Resources.Load("Map/" + xmlFilePath + "/" + _fileName);
        XmlDocument xmlDoc = new XmlDocument();
        //Debug.Log(txtAsset.text);
        xmlDoc.LoadXml(txtAsset.text);

        // 하나씩 가져오기 테스트 예제.
        //XmlNodeList cost_Table = xmlDoc.GetElementsByTagName("cost");
        //foreach (XmlNode cost in cost_Table)
        //{
        //    Debug.Log("[one by one] cost : " + cost.InnerText);
        //}

        // 전체 아이템 가져오기 예제.
        XmlNodeList all_nodes = xmlDoc.SelectNodes("dataroot/MapInfo/Map");
        GameManager.instance.mapInfo = new Info_Map();
        List<Info_Map.BlockInfo> _mapBlockInfo = new List<Info_Map.BlockInfo>();
        foreach (XmlNode node in all_nodes)
        {
            // 수량이 많으면 반복문 사용.
            Info_Map.BlockInfo blockInfo = new Info_Map.BlockInfo(Int32.Parse(node.SelectSingleNode("index").InnerText), Int32.Parse(node.SelectSingleNode("type").InnerText), Int32.Parse(node.SelectSingleNode("height").InnerText));
            _mapBlockInfo.Add(blockInfo);
            //Debug.Log("[at once] index :" + node.SelectSingleNode("index").InnerText);
            //Debug.Log("[at once] type : " + node.SelectSingleNode("type").InnerText);
            //Debug.Log("[at once] height : " + node.SelectSingleNode("height").InnerText);
        }
        GameManager.instance.mapInfo.MapBlockInfo = _mapBlockInfo;
    }

    // Resources/Map1/Map.Xslx 파일.
    //public static void ReadExcelData(string path)  //D:\test\test.xslx
    //{
    //    Excel.Application excelApp = null;
    //    Excel.Workbook wb = null;
    //    Excel.Worksheet ws = null;
    //    try
    //    {
    //        excelApp = new Excel.Application();
    //        wb = excelApp.Workbooks.Open(path); //Open(@"D:\test\test.xslx"); , @"C:\Temp\test.xlsx"
    //        // 첫번째 Worksheet
    //        ws = wb.Worksheets.get_Item(1) as Excel.Worksheet;
    //        // 현재 Worksheet에서 사용된 Range 전체를 선택 : ws.UsedRange; 시작, ws.Range["A2","D9"];, Excel.Range rng = ws.Range["A2"];
    //        Excel.Range rng = ws.Range[ws.Cells[0, 0], ws.Cells[49, 49]];

    //        // Range 데이타를 배열 (One-based array)로
    //        System.Object[,] data = (System.Object[,])rng.Value;
    //        List<Info_Map.BlockInfo> _mapBlockInfo = new List<Info_Map.BlockInfo>();
    //        for (int r = 1, i = 0; r <= data.GetLength(0); r++)
    //        {
    //            for (int c = 1; c <= data.GetLength(1); c++)
    //            {
    //                if (data[r, c] == null)
    //                {
    //                    continue;
    //                }
    //                Info_Map.BlockInfo blockInfo = new Info_Map.BlockInfo(Int32.Parse(data[r, c].ToString()), Int32.Parse(data[r, c].ToString()), Int32.Parse(data[r, c].ToString()));
    //                _mapBlockInfo.Add(blockInfo);
    //                // Data 빼오기
    //                // data[r, c] 는 excel의 (r, c) 셀 입니다.
    //                // data.GetLength(0)은 엑셀에서 사용되는 행의 수를 가져오는 것이고,
    //                // data.GetLength(1)은 엑셀에서 사용되는 열의 수를 가져오는 것입니다.
    //                // GetLength와 [ r, c] 의 순서를 바꿔서 사용할 수 있습니다.
    //            }
    //        }
    //        GameManager.instance.mapInfo.MapBlockInfo = _mapBlockInfo;
    //        wb.Close(true);
    //        excelApp.Quit();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    finally
    //    {
    //        ReleaseExcelObject(ws);
    //        ReleaseExcelObject(wb);
    //        ReleaseExcelObject(excelApp);
    //    }
    //}
    //private static void ReleaseExcelObject(object obj)
    //{
    //    try
    //    {
    //        if (obj != null)
    //        {
    //            Marshal.ReleaseComObject(obj);
    //            obj = null;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        obj = null;
    //        throw ex;
    //    }
    //    finally
    //    {
    //        GC.Collect();
    //    }
    //}
}
