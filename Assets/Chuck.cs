using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Chuck : DestroyableObject
{
    int startHp;
    Transform target;
    AudioSource audio;
    SpriteRenderer sprite;
    [SerializeField]
    Sprite[] sprites;
    [SerializeField]
    Transform healthBar;
    float barStartScaleX;
    Rigidbody2D rigidbody;
    enum AnimationState
    {
        Idle, Idle1, Idle2, Idle3,Idle4,WTF,LazyHit,FingerrHit,Rest,Scratch,ReadNewspapper,LegHit,AirLegHit,FightPose1,Hit,FightPose,Uppercode
    }
    void SetAnimation(AnimationState animation)
    {
        sprite.sprite = sprites[(int)animation];
    }
    // Start is called before the first frame update
    void Start()
    {
        afk = true;
        startHp = hp;
        if(healthBar != null)barStartScaleX = healthBar.transform.localScale.x;
        sprite = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;
    }
    public static bool afk = true;
    bool oneplay = false;
    float toIdle;
    bool charge = false;
    float chargecd = 1;
    bool dead;
    // Update is called once per frame
    IEnumerator Die()
    {
        dead = true;
        Destroy(healthBar.gameObject);
        int i = 0;
        while (i < 20)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f-i/20f);
            yield return new WaitForSeconds(0.1f);
            i++;
        }
        Destroy(gameObject);
    }
    void Update()
    {
        if (!dead)
        {
            if (text != null)
                text.text = "" + hp;
            if (hp <= 0) StartCoroutine(Die());
            if (target != null)
            {
                if (target.position.x >= transform.position.x) transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
                else transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
            }
            if (hp != startHp)
            {
                afk = false;
                if (!oneplay)
                {
                    oneplay = true;
                    if (Camera.main.GetComponent<ShakeBySound>())
                        Camera.main.GetComponent<ShakeBySound>().audioSource.Play();
                }
            }
            if (!afk)
            {
                if (work)
                {
                    if (target != null)
                        transform.position += (target.position - transform.position).normalized * Time.deltaTime * 3.5f * Mathf.Sqrt(Game.difficult);
                }
                if (healthBar != null) healthBar.GetComponent<Renderer>().material.SetFloat("_Modifer", (float)hp / startHp);
                toIdle -= Time.deltaTime;
                if (toIdle <= 0)
                {
                    sprite.flipX = false;
                    SetAnimation(AnimationState.FightPose);
                }

                if (charge)
                {
                    StartCoroutine(Charge());
                    charge = false;
                }
                chargecd -= Time.deltaTime;
                if (chargecd <= 0)
                    charge = true;
            }
            collideParticle.transform.position = new Vector3(collideParticle.transform.position.x, collideParticle.transform.position.y, -0.12f);
        }
    }
    bool work;
    IEnumerator Charge()
    {
        if (work == false)
        {
            work = true;
            if(dead) yield break;
            toIdle = 1.5f;
            SetAnimation(AnimationState.AirLegHit);
            rigidbody.AddForce(transform.up * 50000*Mathf.Sqrt(Game.difficult));
            yield return new WaitForSeconds(0.5f);
            rigidbody.velocity = Vector3.zero;
            Vector3 calcpos;    
            if (target != null)
                calcpos = target.position;
            else calcpos = transform.position;
            yield return new WaitForSeconds(0.25f/Mathf.Sqrt(Game.difficult));         
            while (Vector3.Distance(calcpos, transform.position) > 0.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, calcpos, Time.deltaTime * 15f*Mathf.Sqrt(Game.difficult));
                yield return null;
            }
            float r = Mathf.Max(1f,Random.Range(10, 30) / (10f*Game.difficult));
            chargecd = r;
            work = false;
        }
    }
    bool player = true;
    [SerializeField]
    TextMesh text;
    [SerializeField]

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!dead)
        {
            if (collision.collider.tag == "Respawn")
            {
                if (target != null)
                    collision.collider.GetComponent<Rigidbody2D>().AddForce((target.transform.position - collision.collider.transform.position).normalized * 4000);
            }
            if (collision.collider.tag == "Player")
            {
                toIdle = 1; sprite.flipX = false;
                int x = 0;
                if (target != null)
                {
                    if (target.position.y - transform.position.y > 0.5f) x = 4;
                    if (target.position.y - transform.position.y < -0.5f) x = 5;
                    if (target.position.y - transform.position.y <= 0.5f && target.position.y - transform.position.y >= -0.5f)
                        x = Random.Range(0, 4);
                }
                switch (x)
                {
                    case 0:
                        SetAnimation(AnimationState.LazyHit);
                        break;
                    case 1:
                        SetAnimation(AnimationState.FingerrHit);
                        break;
                    case 2:
                        SetAnimation(AnimationState.Hit);
                        break;
                    case 3:
                        SetAnimation(AnimationState.LegHit);
                        break;
                    case 4:
                        sprite.flipX = true;
                        SetAnimation(AnimationState.Uppercode);
                        break;
                    case 5:
                        SetAnimation(AnimationState.AirLegHit);
                        break;
                }
                if (player)
                {

                    player = false;
                    PlayerPrefs.SetInt("Attempts", PlayerPrefs.GetInt("Attempts") + 1);
                    if (target.GetComponent<Player>()) target.GetComponent<Player>().Dead();
                }
                if (target != null)
                    target.GetComponentInChildren<ParticleSystem>().Play();
                audio.PlayOneShot(audio.clip);
                Camera.main.GetComponent<CameraShake>().StartCoroutine(Camera.main.GetComponent<CameraShake>().Shake(0.2f, 1f));

                collision.collider.GetComponent<Rigidbody2D>().AddForce((collision.collider.transform.position - transform.position) * 2000);
                target = null;
            }
        }
    }
}
