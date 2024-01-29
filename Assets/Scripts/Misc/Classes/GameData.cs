public class GameData
{
    public float AttackDelayDuration = 3;
    public float SkillDelayDuration = 4;

    public int Score;
    public int MutationsAmount;

    public void OnMutated()
    {
        AttackDelayDuration++;
        SkillDelayDuration++;
        MutationsAmount++;
    }

    public void ResetMutation()
    {
        AttackDelayDuration = 3f;
        SkillDelayDuration = 4;
    }
}
