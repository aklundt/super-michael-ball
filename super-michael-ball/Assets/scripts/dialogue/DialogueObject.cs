using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Dialogue Object")]

public class DialogueObject : ScriptableObject {
    // TextArea makes strings more readable in the inspector. It specifies what line count a string should reach that the editor will make a scroll bar instead of expanding the box the text is shown in.
    [SerializeField] [TextArea(1,4)] private string[] dialogue; // creates an array of strings that each hold lines of dialogue
    [SerializeField] private string actorName;

    public string ActorName => actorName;
    public string[] Dialogue => dialogue; // makes a public copy and getter method for our dialogue array. The code says: set index i of Dialogue to index i of dialogue

}
