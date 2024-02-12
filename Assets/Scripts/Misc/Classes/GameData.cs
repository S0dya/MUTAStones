public class GameData
{
    public float AttackDelayDuration = 3;
    public float SkillDelayDuration = 4;

    public int TotalTime;
    public int MutationsAmount;
    public int SkillsAmount;
    public float MaxSize = 1;

    public int Score;

    public void OnMutated() => MutationsAmount++;
    public void OnNewSkill() => SkillsAmount++;
}
