using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : Game
{
    enum Hero
    {
        Freeman,Neo
    }

    [SerializeField]
    Hero hero;
    public int hp = 1;
    public float maxSpeed = 0;
    float moveSpeed = 1;
    public float jumpFocre = 1;
    [SerializeField]
    Sprite[] body;
    [SerializeField]
    SpriteRenderer bodysprite;

    Vector2 axis;
    Animator animator;
    Rigidbody2D rigidbody;
    [SerializeField]
    Weapon[] weapons;
    Weapon weapon;
    WeaponName previousWeapon = 0;
    bool canjump = true;
    int bodyState;
    float jumpcd = 0.2f;
    float jumptimer;
    public static bool dead = false;
    Renderer bulletBar;
    public static bool close;
    private void Awake()
    {
        close =false;
        if(transform.Find("BulletBar").gameObject.GetComponent<Renderer>())
            bulletBar = transform.Find("BulletBar").gameObject.GetComponent<Renderer>();
        maxSpeed *=Mathf.Sqrt(difficult);
        moveSpeed = maxSpeed;
        dead = false;
        if (GetComponent<Animator>())
            animator = GetComponent<Animator>();
        if (GetComponent<Rigidbody2D>())
            rigidbody = GetComponent<Rigidbody2D>();
        for(int i = 0; i < weapons.Length;i++)
        {
            weapons[i].Disable();
            weapons[i] = Instantiate(weapons[i],transform);          
        }
    }
    void FixedUpdate()
    {
     //   transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + new Vector2(1, 0) * axis.x, Time.fixedDeltaTime * moveSpeed);
        jumptimer -= Time.fixedDeltaTime;
     
    }
    bool canshoot = true;
    float[] magazines = new float[(int)WeaponName.Count];
    private void Update()
    {
        if (Input.GetButton("Jump"))
        {
            if (canjump && jumptimer <= 0)
            {
                if (!dead)
                {
                    rigidbody.velocity = Vector3.zero;
                    rigidbody.AddForce(transform.up * jumpFocre * 300);
                    canjump = false;
                    jumptimer = jumpcd;
                }
            }
        }
        if (!dead)
            rigidbody.velocity = new Vector2(axis.x * moveSpeed, rigidbody.velocity.y);
        else
        {
            SpriteRenderer[] col = GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < col.Length; i++)
            {
                if(col[i].color.a> 0.4f) col[i].color = new Color(col[i].color.r, col[i].color.g, col[i].color.b, col[i].color.a - Time.deltaTime/2);
            }
        }
        axis.x = Input.GetAxis("Horizontal");
        axis.y = Input.GetAxis("Vertical");
        Animate();

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            StartCoroutine(crowbar(bodyState));
        }
        if (canshoot)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                SwitchWeapon(WeaponName.Null);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SwitchWeapon(WeaponName.Pistol);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwitchWeapon(WeaponName.AK);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SwitchWeapon(WeaponName.Minigun);
            }

            if (Input.GetButton("Fire1"))
            {
                if (bodyState != 0)
                {
                    if ((int)magazines[(int)previousWeapon] < weapon.Settings.oboima)
                    {
                        magazines[(int)previousWeapon] += weapon.Shoot();
                    }
                }
            }
        }
        for (int i = 0; i < (int)WeaponName.Count; i++)
        {
            if (magazines[i] > 0) magazines[i] -= (Time.deltaTime / 12) * weapons[i - 1].Settings.oboima;
            else magazines[i] = 0;
        }
        if (weapon != null)
        {
            if (bulletBar != null)
            {
                if (1 - magazines[(int)previousWeapon] / weapon.Settings.oboima > 0)
                    bulletBar.material.SetFloat("_Modifer", 1 - magazines[(int)previousWeapon] / weapon.Settings.oboima);
                else
                {
                    bulletBar.material.SetFloat("_Modifer", 0);
                }
            }
        }

        Vector3 mouseAxis;
        mouseAxis = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (!dead)
        {
            if (-transform.position.x + mouseAxis.x > 0) transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
            if (-transform.position.x + mouseAxis.x < 0) transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
        }
    }
    IEnumerator Close()
    {
        yield return new WaitForSeconds(0.75f);
        close = true;
    }
    IEnumerator crowbar(int bodystate)
    {
        if (canshoot)
        {
            Transform crowbar = transform.Find("crowbar");
            Vector3 crowbarpostion = crowbar.localPosition;
            Quaternion crowbarrot = crowbar.localRotation;
            canshoot = false;
            if(weapon!= null)weapon.Disable();
            if (hero == Hero.Freeman) bodyState = 3;
            if (hero == Hero.Neo) bodyState = 1;
            if (hero == Hero.Freeman)
            crowbar.GetComponent<SpriteRenderer>().enabled = true;
            crowbar.GetComponent<CircleCollider2D>().enabled = true;
            float x = 0;
            if (hero == Hero.Freeman) x = 0.1f;
            if (hero == Hero.Neo) x = 0.05f;
            yield return new WaitForSeconds(x);
            crowbar.localPosition = new Vector3(0.436f, -0.106f);
            crowbar.localEulerAngles = new Vector3(0, 0, -110);
            crowbar.GetComponent<SpriteRenderer>().sortingOrder = 2;
            if (hero == Hero.Freeman) bodyState = 1;
            if (hero == Hero.Neo) bodyState = 3;
            if (hero == Hero.Freeman) x = 0.25f;
            if (hero == Hero.Neo) x = 0.25f;
            yield return new WaitForSeconds(x);
            canshoot = true;
            if(weapon!= null)weapon.Enable();
            if (hero == Hero.Freeman)
            transform.Find("crowbar").GetComponent<SpriteRenderer>().enabled = false;
            crowbar.GetComponent<CircleCollider2D>().enabled = false;
            bodyState = bodystate;
            crowbar.localPosition = crowbarpostion;
            crowbar.localRotation = crowbarrot;
            crowbar.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
    }
    void Animate()
    {
        if (animator != null)
        {
            if (axis.x != 0) animator.SetInteger("State", 1);
            else animator.SetInteger("State", 0);

            animator.SetBool("Jump", !canjump);
            if (body.Length > 0)
            {
                bodysprite.sprite = body[bodyState];
            }
        }
    }
    enum WeaponName
    {
        Null, Pistol, AK,Minigun,Count
    }
    void SwitchWeapon(WeaponName weaponName)
    {
        if (weaponName != previousWeapon)
        {
            moveSpeed = maxSpeed;
            if (weapon != null) weapon.Disable();
            if (weaponName > 0)
            {             
                       
                if (hero == Hero.Neo)
                {
                    weapon = weapons[(int)weaponName - 1];
                    weapons[(int)weaponName - 1].Enable();
                    if (weapon.Settings.fireRate*1.25f <= 20)
                        weapon.Settings.fireRate = weapon.Settings.fireRate * 1.25f;
                    else
                    {
                        weapon.Settings.fireRate = 20;
                    }
                }
                else
                {
                    weapon = weapons[(int)weaponName - 1];
                    weapons[(int)weaponName - 1].Enable();
                    moveSpeed = (maxSpeed / weapon.Settings.speedDecrease);
                }
            }
            else
            {
                weapon = null;
            }
            if (weapon != null)
            {
                if (!weapon.Settings.two_Handed)
                {
                    bodyState = 1;
                }
                else
                {
                    bodyState = 2;
                }
            }
            else
                bodyState = 0;
            if (weapon != null)
            {
                weapon.AudioPlay();
            }
            previousWeapon = weaponName;
        }
    }
    [SerializeField]
    PhysicsMaterial2D bouncy;
    public void Dead()
    {
        if (bulletBar != null)
            Destroy(bulletBar.gameObject);
        if (!dead)
        {
            rigidbody.freezeRotation = false;
            rigidbody.AddTorque(100 * Mathf.Sign(Random.Range(-1, 1)));
            GetComponent<Collider2D>().sharedMaterial = bouncy;
            StartCoroutine(Close());
        }
        dead = true;
        Destroy(gameObject.GetComponent<Player>(), 1);
        Destroy(gameObject.GetComponent<Animator>(), 1);
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.collider.tag != "EditorOnly")canjump = true;
      
    }
    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.collider.tag == "Platform")
        {
            if (Input.GetKey(KeyCode.LeftShift)|| Input.GetKey(KeyCode.S))
            {                
                StartCoroutine(shiftPlatform(col.collider,0.4f));
            }    
        }
        if (col.collider.tag == "Respawn")
        {
            Rigidbody2D rb = col.collider.GetComponent<Rigidbody2D>();
            if (rb.velocity.magnitude*Mathf.Sqrt(rb.mass) > 15F/ col.collider.GetComponent<DestroyableObject>().tverdost)
            {
                if (!dead)
                {
                    col.collider.GetComponent<DestroyableObject>().hp -= 10;
                    if (col.collider.GetComponent<DestroyableObject>().hp > 0) col.collider.GetComponent<DestroyableObject>().PlayAudio();
                    GetComponentInChildren<ParticleSystem>().Play();
                    Dead();
                    GetComponent<Rigidbody2D>().freezeRotation = false;
                    GetComponent<Rigidbody2D>().AddTorque(100 * Mathf.Sign(Random.Range(-1, 1)));
                    GetComponent<Rigidbody2D>().AddForce((col.collider.transform.position - transform.position) * -1000);
                }
            }
        }
    }
    IEnumerator shiftPlatform(Collider2D collider,float delay)
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collider);
        yield return new WaitForSeconds(delay);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collider, false);

    }
}
