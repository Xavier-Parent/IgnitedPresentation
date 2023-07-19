using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastingSpell : MonoBehaviour
{
    [SerializeField]
    private GameObject m_FireBall;
    [SerializeField]
    private GameObject m_LightningBolt;
    [SerializeField]
    private GameObject m_Enemy;

    [SerializeField]
    private Transform m_SpellSpawn;

    private float m_ProjectileForce = 10;

    private void Update()
    {
        Projectile();
    }

    private void Projectile()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Rigidbody m_Projectile = Instantiate(m_FireBall, m_SpellSpawn.position, Quaternion.identity).GetComponent<Rigidbody>();
            m_Projectile.AddForce((m_Enemy.transform.position - m_SpellSpawn.position) * m_ProjectileForce, ForceMode.VelocityChange);
            Destroy(m_Projectile.gameObject, 4);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            Rigidbody m_Projectile = Instantiate(m_LightningBolt, m_SpellSpawn.position, Quaternion.identity).GetComponent<Rigidbody>();
            m_Projectile.AddForce((m_Enemy.transform.position - m_SpellSpawn.position) * m_ProjectileForce, ForceMode.VelocityChange);
            Destroy(m_Projectile.gameObject, 4);
        }
    }
}
