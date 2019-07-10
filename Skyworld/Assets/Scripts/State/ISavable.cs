// Interface for state-saved object data
interface ISavable
{
    // Save object fields in a record
    StateRecord GetRecord();

    // Load record data into object fields
    void SetData(StateRecord loaded);
}
