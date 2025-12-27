using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;

public class DataTableManager : SingletonBehaviour<DataTableManager>
{
    private const string DATA_PATH = "DataTable";


    protected override void Init()
    {
        base.Init();

        LoadObjectDataTable();
        LoadScoreDataTable();
    }

    #region Object_Data

    private const string OBJECT_DATA_TABLE = "ObjectDataTable";
    private List<ObjectData> ObjectDataTable = new List<ObjectData>();

    private void LoadObjectDataTable()
    {
        var parsedDataTable = CSVReader.Read($"{DATA_PATH}/{OBJECT_DATA_TABLE}");

        foreach (var data in parsedDataTable)
        {
            var objectData = new ObjectData
            {
                Object_ID = Convert.ToInt32(data["Object_ID"]),
                Name = data["Name"].ToString(),
                MaxDurability = Convert.ToInt32(data["MaxDurability"]),
                ScoreTriggerType = Convert.ToInt32(data["ScoreTriggerType"]),
                ScoreValue = Convert.ToInt32(data["ScoreValue"]),
                BreakLevels  = Convert.ToInt32(data["BreakLevels"]),
            };

            ObjectDataTable.Add(objectData);
        }
    }

    public ObjectData GetObjectData(int id)
    {
        return ObjectDataTable.Where(item => item.Object_ID == id).FirstOrDefault();
    }
    #endregion

    #region Score_Data

    private const string SCORE_DATA_TABLE = "ScoreDataTable";
    private List<ScoreData> ScoreDataTable = new List<ScoreData>();

    private void LoadScoreDataTable()
    {
        var parsedDataTable = CSVReader.Read($"{DATA_PATH}/{SCORE_DATA_TABLE}");

        foreach (var data in parsedDataTable)
        {
            var scoreData = new ScoreData
            {
                Stage = Convert.ToInt32(data["Stage"]),
                Gaol_Score = Convert.ToInt32(data["Gaol_Score"]),
                MaxTime = Convert.ToInt32(data["MaxTime"]),
            };

            ScoreDataTable.Add(scoreData);
        }
    }

    public ScoreData GetScoreData(int stage)
    {
        return ScoreDataTable.Where(item => item.Stage == stage).FirstOrDefault();
    }
    #endregion
}

public class ObjectData
{
    public int Object_ID;
    public string Name;
    public int MaxDurability;
    public int ScoreTriggerType;
    public int ScoreValue;
    public int BreakLevels;
}

public class ScoreData
{
    public int Stage;
    public int Gaol_Score;
    public int MaxTime;
}