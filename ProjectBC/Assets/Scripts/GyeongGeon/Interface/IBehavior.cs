public interface IBehavior
{
    void Move();
    void Attack();
    void Damageable(Character target, float _damage);
    bool Die();
    bool Alive();
}
