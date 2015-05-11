using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Reflection;
using System;
using System.IO;
using System.Collections.Generic;

public enum ItemType
{
    None,
    Root,
    SkillEvent,
    SkillAction
}

public enum SkillEventType
{
    ActiveEvent,
    AnimaEvent 
}

public class SkillBase
{
    public ItemType type = ItemType.None;
    public string resPath = "";
    public int skillId = 0;
}

public class SkillEditor : EditorWindow
{
    static TreeViewControl m_treeViewControl = null;
    static TreeViewItem _root = null;
    static TreeViewItem _curItem = null;
    static string FixPath = "Assets/Resources/FightData/Skill/"; //技能数据的位置
    static string ResPath = "FightData/Skill/";
    static List<string> _skillActionName = new List<string>();
    static Dictionary<int, Skill> _skillDic = new Dictionary<int, Skill>();

    [MenuItem("GameEditor/Skill Editor")]
    public static void ShowSkillTreeViewPanel()
    {
        _skillDic.Clear();
        GetSkillActionName();
        GetSkillData();
        CreateTreeView();
        RefreshPanel();
    }

    static SkillEditor m_instance = null;

    public static SkillEditor GetPanel()
    {
        if (null == m_instance)
        {
            m_instance = EditorWindow.GetWindow<SkillEditor>(false, "技能编辑器", false);
        }
        return m_instance;
    }

    public static void RefreshPanel()
    {
        SkillEditor panel = GetPanel();
        panel.Repaint();
    }

    static void CreateTreeView()
    {     
        m_treeViewControl = TreeViewInspector.AddTreeView();
        m_treeViewControl.DisplayInInspector = false;
        m_treeViewControl.DisplayOnGame = false;
        m_treeViewControl.DisplayOnScene = false;
        m_treeViewControl.X = 600;
        m_treeViewControl.Y = 500;       

        _root = m_treeViewControl.RootItem;
        _root.Header = "所有技能";
        SkillBase data1 = new SkillBase();
        data1.type = ItemType.None;
        _root.DataContext = data1;
        _curItem = _root;
        AddEvents(_root);

        CreateSkillItem();
    }

    static void AddEvents(TreeViewItem item)
    {
        AddHandlerEvent(out item.Selected);
    }

    public static void Handler(object sender, System.EventArgs args)
    {
        _curItem = sender as TreeViewItem;
        Selection.activeObject = Resources.Load((_curItem.DataContext as SkillBase).resPath);
    }

    static void AddHandlerEvent(out System.EventHandler handler)
    {
        handler = new System.EventHandler(Handler);
    }

    void OnEnable()
    {
        wantsMouseMove = true;
    }

    int skillId = 0; //技能id
    int selectIdx = 0; //选择的事件
    void OnGUI()
    {
        if (null == m_treeViewControl)
        {
            return;
        }

        if (_curItem == null)
        {
            return;
        }

        wantsMouseMove = true;
        if (null != Event.current &&
            Event.current.type == EventType.MouseMove)
        {
            Repaint();
        }
        m_treeViewControl.DisplayTreeView(TreeViewControl.DisplayTypes.USE_SCROLL_VIEW);

        if ((_curItem.DataContext as SkillBase).type == ItemType.None)
        {
            skillId = EditorGUILayout.IntField("技能id:",skillId);
            GUILayout.BeginVertical();
            if (GUILayout.Button("创建技能"))
            {
                AddSkill(skillId);
            }
            GUILayout.EndVertical();
        }
        else if ((_curItem.DataContext as SkillBase).type == ItemType.Root)
        {
            GUILayout.BeginHorizontal();
            string[] list = new string[] { "-------请选择------", SkillEventType.AnimaEvent.ToString() };
            selectIdx = EditorGUILayout.Popup("选择事件", selectIdx, list);
            if (GUILayout.Button("添加事件"))
            {
                AddSkillEventNode(_curItem, list[selectIdx]);
            }
            GUILayout.EndHorizontal();
        }
        else if ((_curItem.DataContext as SkillBase).type == ItemType.SkillEvent)
        {
            GUILayout.BeginHorizontal();
            List<string> list = new List<string>();
            list.Add("-------请选择------");
            list.AddRange(_skillActionName);
            selectIdx = EditorGUILayout.Popup("选择行为", selectIdx, list.ToArray());
            if (GUILayout.Button("添加行为"))
            {
                AddSkillAction(_curItem, list[selectIdx]);
            }
            GUILayout.EndHorizontal();
        }
    }

    /// <summary>
    /// 添加技能
    /// </summary>
    /// <param name="newSkillId">技能id</param>
    /// <param name="isCreateAsset">是否创建新的资源</param>
    static TreeViewItem AddSkill(int newSkillId, bool isCreateAsset = true)
    {
        Skill skill = ScriptableObject.CreateInstance<Skill>();
        _skillDic[skill.skillId] = skill;
        skill.skillId = newSkillId;
        if (isCreateAsset)
        {
            AssetEditor.CreateAsset(skill, FixPath + newSkillId, "Skill");
        }

        TreeViewItem skillItem = _root.AddItem(newSkillId.ToString());
        SkillBase data = new SkillBase();
        data.type = ItemType.Root;
        data.resPath = ResPath + newSkillId + "/Skill";
        data.skillId = newSkillId;
        skillItem.DataContext = data;
        AddEvents(skillItem);
        if (isCreateAsset)
        {
            TreeViewItem evtItem = AddSkillEventNode(skillItem, SkillEventType.ActiveEvent.ToString());
            if (evtItem != null)
            {
                AddSkillAction(evtItem, "PlayAnimator");
                AddSkillAction(evtItem, "FinishSkill");
            }
            AddSkillEventNode(skillItem, SkillEventType.AnimaEvent.ToString());
        }

        return skillItem;
    }

    /// <summary>
    /// 添加事件
    /// </summary>
    /// <param name="item">父节点</param>
    /// <param name="name">事件名</param>
    /// <param name="isCreateAsset">是否创建新的资源</param>
    /// <returns></returns>
    static TreeViewItem AddSkillEventNode(TreeViewItem item, string name, bool isCreateAsset = true)
    {
        if (name == String.Empty)
        {
            return null;
        }
        Skill skill = RegetditSkill(item);
        if (skill == null)
        {
            return null;
        }
        string evtName = name;
        Assembly ass = typeof(SkillEvent).Assembly;
        string[] nameList = name.Split('_');
        System.Type type = ass.GetType(nameList[0]);
        SkillEvent evt = System.Activator.CreateInstance(type) as SkillEvent;
        SkillBase evtData = new SkillBase();
        if (evt is ActiveEvent)
        {
            if (isCreateAsset)
            {
                skill.activeEvent.Add(evt as ActiveEvent);
            }         
        }
        else
        {
            if (skill.animaEvent.Count > 0)
            {
                evtName = name + "_" + skill.animaEvent.Count;
            }
            if (isCreateAsset)
            {
                skill.animaEvent.Add(evt as AnimaEvent);
            }          
        }
        if (isCreateAsset)
        {
            AssetEditor.CreateAsset(evt, FixPath + skill.skillId + "/" + evtName, evtName);
        }
        else
        {
            evtName = name;
        }
        TreeViewItem evtItem = item.AddItem(evtName);                   
        evtData.type = ItemType.SkillEvent;
        evtData.skillId = skill.skillId;
        evtData.resPath = ResPath + skill.skillId + "/" + evtName + "/" + evtName;
        evtItem.DataContext = evtData;
        AddEvents(evtItem);
        EditorUtility.SetDirty(skill);
        return evtItem;
    }

    /// <summary>
    /// 添加技能action
    /// </summary>
    /// <param name="item">父节点</param>
    /// <param name="name">action名</param>
    /// <param name="isCreateAsset">是否创建新资源</param>
    static void AddSkillAction(TreeViewItem item, string name, bool isCreateAsset = true)
    {
        SkillBase data = item.DataContext as SkillBase;
        SkillBase actData = new SkillBase();
        Skill skill = RegetditSkill(item);
        SkillEvent evt = Resources.Load(data.resPath) as SkillEvent;
        if (skill == null || evt == null)
        {
            return;
        }
        Assembly ass = typeof(SkillAction).Assembly;
        string[] nameList = name.Split('_');
        System.Type type = ass.GetType(nameList[0]);
        SkillAction act = System.Activator.CreateInstance(type) as SkillAction;
        
        string actName = name;
        int num = 0;
        foreach (var act2 in evt.skillActions)
        {
            if (act2.name.StartsWith(name))
            {
                num++;
            }
        }
        if (num > 0)
        {
            actName = actName + "_" + num;
        }
        if (isCreateAsset)
        {
            evt.skillActions.Add(act);
            AssetEditor.CreateAsset(act, FixPath + skill.skillId + "/" + evt.name + "/SkillAction", actName);
        }
        else
        {
            actName = name;
        }
       
        TreeViewItem evtItem = item.AddItem(actName);
        actData.type = ItemType.SkillAction;
        actData.skillId = skill.skillId;
        actData.resPath = ResPath + skill.skillId + "/" + evt.name + "/SkillAction/" + actName;
        evtItem.DataContext = actData;
        AddEvents(evtItem);
        EditorUtility.SetDirty(evt);
        EditorUtility.SetDirty(skill);
    }

    void OnDestroy()
    {
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    static Skill RegetditSkill(TreeViewItem item)
    {
        SkillBase data = item.DataContext as SkillBase;
        Skill skill = null;
        if (_skillDic.ContainsKey(data.skillId))
        {
            skill = _skillDic[data.skillId];
        }
        else
        {
            skill = Resources.Load(data.resPath) as Skill;
            if (skill != null)
            {
                _skillDic[data.skillId] = skill;
            }
        }
        return skill;
    }

    static void CreateSkillItem()
    {
        List<Skill> skills = new List<Skill>();
        foreach (var node in _skillDic)
        {
            if (node.Value != null)
            {
                skills.Add(node.Value);
            }
        }
        for (int i = 0; i < skills.Count; ++i)
        {
            TreeViewItem skillItem = AddSkill(skills[i].skillId, false);
            if (skillItem == null)
            {
                continue;
            }
            for (int n = 0; n < skills[i].activeEvent.Count; n++)
            {
                TreeViewItem evtItem = AddSkillEventNode(skillItem, skills[i].activeEvent[n].name, false);
                for (int m = 0; m < skills[i].activeEvent[n].skillActions.Count; m++)
                {
                    AddSkillAction(evtItem, skills[i].activeEvent[n].skillActions[m].name, false);
                }
            }

            for (int n = 0; n < skills[i].animaEvent.Count; n++)
            {
                TreeViewItem evtItem = AddSkillEventNode(skillItem, skills[i].animaEvent[n].name, false);
                for (int m = 0; m < skills[i].animaEvent[n].skillActions.Count; m++)
                {
                    AddSkillAction(evtItem, skills[i].animaEvent[n].skillActions[m].name, false);
                }
            }
        }
    }

    static void GetSkillData()
    {
        try
        {
            DirectoryInfo parentFolder = new DirectoryInfo(FixPath);
            //遍历文件夹
            foreach (DirectoryInfo folder in parentFolder.GetDirectories())
            {
                Skill skill = Resources.Load("FightData/Skill/" + folder.Name + "/Skill") as Skill;
                if (skill == null)
                {
                    continue;
                }
                _skillDic[skill.skillId] = skill;
            }  
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }
    }

    static void GetSkillActionName()
    {
        try
        {
            _skillActionName.Clear();
            int i = 0;
            string _skillActionPath = "Assets/Script/Skill/SkillEvent/SkillAction/";
            DirectoryInfo parentFolder = new DirectoryInfo(_skillActionPath);
            //遍历文件夹
            foreach (var file in parentFolder.GetFiles())
            {
                if (file.Extension != ".cs" || file.Name == "SkillAction.cs")
                {
                    continue;
                }
                _skillActionName.Add(file.Name.Replace(file.Extension, ""));                 
                 ++i;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}
