namespace Game
{
    public class GameObject
    {
        protected string textura;

        protected Transform transform;

        protected Animation idle;
        protected Animation currentAnimation;

        public float PosY => transform.position.y;
        public float PosX => transform.position.x;

        public Transform Transform => transform;

        protected int RealWidth => (int)(currentAnimation.CurrentFrame.Width * transform.scale.x);
        protected int RealHeight => (int)(currentAnimation.CurrentFrame.Height * transform.scale.y);

        public Vector2 RealSize => new Vector2(RealWidth, RealHeight);


        public GameObject(float p_sizeX, float p_sizeY, string p_textura,
            int p_posicionX, int p_posicionY)
        {
            transform = new Transform(p_posicionX, p_posicionY, p_sizeX, p_sizeY);
            textura = p_textura;

            GameUpdateManager.Instance.AddUpdatableObj(this);
        }

        public virtual void Input()
        {

        }

        public virtual void Update()
        {
            currentAnimation.Update();
        }

        public virtual void Draw()
        {
            Engine.Draw(currentAnimation.CurrentFrame, transform.position.x, transform.position.y, transform.scale.x, transform.scale.y, 90);
        }
    }
}