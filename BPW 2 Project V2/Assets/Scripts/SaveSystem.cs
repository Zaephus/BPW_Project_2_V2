using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour {

    private string path;

    public void SetPath(string fileName) {
        if(Application.isEditor) {
            path = Application.dataPath + "/SaveData/" + fileName + ".txt";
        }
        else {
            path = Application.persistentDataPath + fileName + ".txt";
        }
    }

    public void SaveUnit(Unit unit,string fileName) {

        SetPath(fileName);

        UnitSaver targetUnit = new UnitSaver();

        targetUnit.unitName = unit.unitName;

        targetUnit.maxHealth = unit.maxHealth;
        targetUnit.currentHealth = unit.currentHealth;

        targetUnit.baseAttackStrength = unit.baseAttackStrength;
        targetUnit.currentAttackStrength = unit.currentAttackStrength;

        targetUnit.baseDefenseStrength = unit.baseDefenseStrength;
        targetUnit.currentDefenseStrength = unit.currentDefenseStrength;

        StreamWriter writer = new StreamWriter(path,false);

        writer.WriteLine(JsonUtility.ToJson(targetUnit,true));
        writer.Close();
        writer.Dispose();

    }

    public void LoadUnit(Unit unit,string fileName) {
            
        SetPath(fileName);

        if(File.Exists(path)) {

            StreamReader reader = new StreamReader(path);

            UnitSaver targetUnit = JsonUtility.FromJson<UnitSaver>(reader.ReadToEnd());

            unit.unitName = targetUnit.unitName;

            unit.maxHealth = targetUnit.maxHealth;
            unit.currentHealth = targetUnit.currentHealth;

            unit.baseAttackStrength = targetUnit.baseAttackStrength;
            unit.currentAttackStrength = targetUnit.currentAttackStrength;

            unit.baseDefenseStrength = targetUnit.baseDefenseStrength;
            unit.currentDefenseStrength = targetUnit.currentDefenseStrength;

            reader.Close();
            reader.Dispose();

        }
        else {
            SaveUnit(unit,fileName);
        }
        
    }

}