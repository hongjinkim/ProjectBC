public interface IBehavior
{
    void Move();
    void Attack();
    void Damageable(Character target, float _damage);
    void Die();
    bool Alive();
}
