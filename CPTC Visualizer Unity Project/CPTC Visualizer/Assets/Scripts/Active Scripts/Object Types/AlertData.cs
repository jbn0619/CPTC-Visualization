public class AlertData
{
    #region Fields
    [Header("JSON Data Fields")]
    [SerializeField]
    public string type;
    [SerializeField]
    public List<int> nodes;
    [SerializeField]
    public int priority;
    [SerializeField]
    public int timestamp;

    #endregion Fields

    public AlertData()
    {
        
    }

    public void SetData(Alert alert)
    {
        type = alert.type;
        nodes = alert.nodes;
        priority = alert.priority;
        timestamp = alert.timestamp;
    }
}