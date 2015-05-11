using UnityEngine;
using System.Collections;

public class Example : MonoBehaviour {

    public TreeViewControl m_myTreeView = null;

	// Use this for initialization
	public void Start () {

//         m_myTreeView = gameObject.GetComponent<TreeViewControl>();
        if (null == m_myTreeView)
        {
            Debug.LogError("Use the tree view menu to add the control");
            return;
        }

        PopulateExampleData(m_myTreeView);
    }

    public static void Handler(object sender, System.EventArgs args)
    {
        Debug.Log(string.Format("{0} detected: {1}", args.GetType().Name, (sender as TreeViewItem).Header));
    }

    static void AddHandlerEvent(out System.EventHandler handler)
    {
        handler = new System.EventHandler(Handler);
    }

    static void AddEvents(TreeViewItem item)
    {
        AddHandlerEvent(out item.Click);
        AddHandlerEvent(out item.Checked);
        AddHandlerEvent(out item.Unchecked);
        AddHandlerEvent(out item.Selected);
        AddHandlerEvent(out item.Unselected);
    }

    public static void PopulateExampleData(TreeViewControl item)
    {
        item.Width = 600;
        item.Height = 500;
        item.Header = "Example.cs populated this treeview control";
        AddEvents(item.RootItem);
        TreeViewItem item1 = item.RootItem.AddItem("You can also add a tree view control");
        AddEvents(item1);
        AddEvents(item1.AddItem("to any existing game object by"));
        AddEvents(item1.AddItem("selecting the game object and"));
        AddEvents(item1.AddItem("using the menu item"));
        TreeViewItem games = item1.AddItem("TreeView->Add Tree View to selected.", false);
        AddEvents(games);
        AddEvents(games.AddItem("You can also drag and drop the"));
        AddEvents(games.AddItem("TreeViewControl script onto a"));
        AddEvents(games.AddItem("game object."));
		AddEvents(games.AddItem("New checked and", false, true));
		AddEvents(games.AddItem("unchecked checkboxes", false, false));
	}

    public void OnGUI()
    {
        if (null != m_myTreeView &&
            null != m_myTreeView.SelectedItem &&
            !string.IsNullOrEmpty(m_myTreeView.SelectedItem.Header))
        {
            GUILayout.Label(string.Format("Example selected: {0}", m_myTreeView.SelectedItem.Header));
        }
    }
}
