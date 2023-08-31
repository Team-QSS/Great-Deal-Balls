using System;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class CloseMonster : MonoBehaviour, IHit
{
    private int index;

    private Collider     body;
    private Collider     realWeapon;
    private NavMeshAgent navAg;
    private Animator     anim;

    [Header("Anim Stats")] [SerializeField]
    private float attackAnimTime;

    [SerializeField] private float attackSP;
    [SerializeField] private float attackEP;
    [SerializeField] private float hitTime;
    [SerializeField] private float dieTime;

    [Header("Base Stats")] public float hp     = 100;
    public                        float damage = 10;
    public                        float speed  = 3;

    private bool isAttack;
    private bool isHit;
    private bool isDie;

    private float attackTimer;
    private float attackST;
    private float attackET;
    private float hitTimer;
    private float dieTimer;

    private void Awake()
    {
        body       = GetComponent<Collider>();
        realWeapon = GetComponentInChildren<BoxCollider>();
        navAg      = GetComponent<NavMeshAgent>();
        anim       = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        navAg.speed   = speed;
        body.enabled  = true;
        navAg.enabled = true;
        isDie         = false;
        isHit         = false;
        isAttack      = false;
    }

    private void Update()
    {
        if (isDie)
        {
            if (Time.time > dieTimer)
            {
                GameManager.MonsterPool.ReturnObject(gameObject, index);
            }

            return;
        }

        if (isAttack)
        {
            if (Time.time > attackST && Time.time < attackET) realWeapon.enabled = true;
            else realWeapon.enabled                                              = false;
            if (Time.time > attackTimer)
            {
                isAttack        = false;
                navAg.isStopped = false;
            }

            return;
        }

        if (isHit)
        {
            if (Time.time > hitTimer)
            {
                isHit           = false;
                navAg.isStopped = false;
            }

            return;
        }

        navAg.enabled = true;
        navAg.SetDestination(PlayerController.PlayerPos);
        anim.SetBool("Run", true);
        navAg.avoidancePriority = Mathf.CeilToInt(navAg.remainingDistance * 10);
    }

    private void Attack()
    {
        if (isAttack || isHit || isDie) return;
        isAttack = true;
        anim.SetBool("Run", false);
        transform.LookAt(PlayerController.PlayerPos);
        navAg.isStopped = true;
        anim.SetTrigger("Attack");
        attackTimer = Time.time + attackAnimTime;
        attackST    = Time.time + attackAnimTime * attackSP;
        attackET    = Time.time + attackAnimTime * attackEP;
    }

    void Die()
    {
        body.enabled  = false;
        navAg.enabled = false;
        anim.Play("Die");
        isDie = true;
        //ToDo:죽음 효과
        dieTimer = Time.time + dieTime;
    }

    public void Hit(float damage)
    {
        if (isDie) return;
        hp -= damage;
        if (!isHit)
        {
            isHit           = true;
            isAttack        = false;
            navAg.isStopped = true;
            anim.ResetTrigger("Attack");
            anim.SetBool("Run", false);
            realWeapon.enabled = false;
        }

        if (hp <= 0)
        {
            Die();
        }
        else
        {
            anim.Play("Hit", 0, 0f);
        }

        hitTimer = Time.time + hitTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<IHit>().Hit(damage);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Close Monster Sensor"))
        {
            Attack();
        }
    }
}