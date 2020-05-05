using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, PLAYERCHOOSEDACTION }

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit enemyUnit;

    public Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleState state;

    public AudioClip hitSound;
    public AudioClip healSound;
    private AudioSource audioSource;

    public GameObject combat;
    public PlayableDirector playerWinTimeline;
    public PlayableDirector playerLoseTimeline;

    // Þegar kveikt er á bardaganum (ef verið er að keppa aftur), núllstilla allt
    void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        // Búa til instance af leikmannsprefabi
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        // Óvinaprefab
        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        dialogueText.text = $"Enemy {enemyUnit.unitName} wants to battle!";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        enemyHUD.SetHP(enemyUnit.currentHP);
        audioSource.PlayOneShot(hitSound);
        dialogueText.text = "The attack is successful!";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(5);

        playerHUD.SetHP(playerUnit.currentHP);
        audioSource.PlayOneShot(healSound);
        dialogueText.text = "You feel renewed strength!";

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        dialogueText.text = $"{enemyUnit.unitName} attacks!";

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
        playerHUD.SetHP(playerUnit.currentHP);
        audioSource.PlayOneShot(hitSound);

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator WaitThenDisable(float waitTime, PlayableDirector endTimeline = null)
    {
        // Eyða bardaga þegar hann er búinn
        yield return new WaitForSeconds(waitTime);
        if (endTimeline)
        {
            endTimeline.Play();
        }
        combat.SetActive(false);
    }

    void EndBattle()
    {
        PlayableDirector endTimeline = null;
        if (state == BattleState.WON)
        {
            dialogueText.text = "You won the battle!";
            if (playerWinTimeline)
            {
                endTimeline = playerWinTimeline;
            }
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated.";
            if (playerLoseTimeline)
            {
                endTimeline = playerLoseTimeline;
            }
        }
        StartCoroutine(WaitThenDisable(2f, endTimeline));
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose an action:";
    }

    public void OnAttackButton() {
        // Ef leikmaður velur árás
        if (state != BattleState.PLAYERTURN && state == BattleState.PLAYERCHOOSEDACTION)
        {
            return;
        }
        state = BattleState.PLAYERCHOOSEDACTION;
        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        // Ef leikmaður velur „heal“
        if (state != BattleState.PLAYERTURN && state == BattleState.PLAYERCHOOSEDACTION)
        {
            return;
        }
        state = BattleState.PLAYERCHOOSEDACTION;
        StartCoroutine(PlayerHeal());
    }
}
