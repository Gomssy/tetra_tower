
public class AttackCombo
{
    float time;
    //시전시간 1.5초
    string name;
    //화염발사!
    string combo;
    string comboAnim;
    //"ABCACBAC"
    public AttackCombo(string name, string combo, float time , string comboAnim)
    {
        this.time = time;
        this.name = name;
        this.combo = combo;
        this.comboAnim = comboAnim;
    }
    public override string ToString()
    {
        return name;
    }
    public override bool Equals(object obj)
    {
        return combo.Equals(obj);
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    public float getTime()
    {
        return time;
    }
    public string getComboAnim()
    {
        return comboAnim;
    }
}