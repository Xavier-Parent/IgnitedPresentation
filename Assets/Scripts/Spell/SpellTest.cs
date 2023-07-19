using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTest : MonoBehaviour
{
    [SerializeField]
    private GameObject[] spells;
    [SerializeField]
    private GameObject m_Enemy;
    [SerializeField]
    private Transform m_SpellSpawn;

    private float m_ProjectileForce = 8;
    public int selectedSpellIndex;  // Index of the currently selected spell

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && selectedSpellIndex >= 0 && selectedSpellIndex < spells.Length)
        {
            // Activate the selected spell
            ActivateSpell(selectedSpellIndex);
        }
    }

    private void ActivateSpell(int spellIndex)
    {
        // Retrieve the spell GameObject using the provided index
        GameObject selectedSpell = spells[spellIndex];

        // Implement the logic to activate the spell
        Rigidbody m_Projectile = Instantiate(selectedSpell, m_SpellSpawn.position, Quaternion.identity).GetComponent<Rigidbody>();
        m_Projectile.AddForce((m_Enemy.transform.position - m_SpellSpawn.position) * m_ProjectileForce, ForceMode.VelocityChange);
        Destroy(m_Projectile.gameObject, 4);
    }
}
