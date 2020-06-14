using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Weapon : Game
{
    [System.Serializable]
    public class WeaponSettings
    {
        public int damageMultiplier = 1;
        public float fireRate = 1f;
        public float SprayAngle = 0f;
        public float speedDecrease = 1f;
        public bool two_Handed = false;
        public int soundid = 0;
        public float shake = 0.075f;
        public float bulletSpeed = 40;
        public float oboima = 6;
    }
    public WeaponSettings Settings;
    [SerializeField]
    ParticleSystem pbullet;
    ParticleSystem.MainModule main;
    [SerializeField]
    Vector3 pivot;
    public AudioClip audio;
    float shootTime;
    float startFireRate;
    private void Awake()
    {
        main = pbullet.main;
        transform.localPosition = pivot;
        startFireRate = Settings.fireRate;
    }
    public void AudioPlay()
    {
        if (audio != null)
        {
            AudioSource a = new GameObject().AddComponent<AudioSource>();
            a.clip = audio;
            a.Play();
            Destroy(a.gameObject, a.clip.length);
        }
    }
    public void Disable()
    {
        GetComponent<SpriteRenderer>().enabled = false;     
        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
        int l = srs.Length;
        if (l > 0)
        {
            for(int i = 0; i < l;i++)
            {
                srs[i].enabled = false;
            }
        }
    }
    public void Enable()
    {
        Settings.fireRate = startFireRate;
        GetComponent<SpriteRenderer>().enabled = true;
        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
        int l = srs.Length;
        if (l > 0)
        {
            for (int i = 0; i < l; i++)
            {
                srs[i].enabled = true;
            }
        }
    }

    public int Shoot()
    {
        if (shootTime <= 0)
        {
            Camera.main.GetComponent<CameraShake>().StartCoroutine(Camera.main.GetComponent<CameraShake>().Shake(0.25f, Settings.shake));
            shootTime = 1/Settings.fireRate;
            //Bullet bul = Instantiate(Settings.bullet, transform.position + shootPivot, transform.rotation);
            main.startSpeed = Settings.bulletSpeed; 
            pbullet.transform.localEulerAngles = new Vector3(0,90,0);
            pbullet.transform.Rotate(Random.Range(-Settings.SprayAngle*100f, Settings.SprayAngle*100f)/100f,0,0);
            if(transform.rotation != Quaternion.identity)
            {
                main.startRotation = 180*Mathf.Deg2Rad;
            }
            else
            {
                main.startRotation =  0;
            }
            pbullet.Play();
            pbullet.GetComponent<ParticlesCollision>().damage = Settings.damageMultiplier;
       
            PlayerPrefs.SetInt("Shots", PlayerPrefs.GetInt("Shots") + 1);

                GameObject.Find("Sounds").GetComponent<AudioSource>().PlayOneShot(GameObject.Find("Sounds").GetComponents<AudioSource>()[Settings.soundid].clip);
            return 1;
        }
        return 0;
    }
    private void Update()
    {
        shootTime -= Time.deltaTime;
    }
}
