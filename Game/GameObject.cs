using System;

namespace Game
{
    public class Renderer
    {
        private Texture texture;

        private Animation currentAnimation;

        private GameObject gameObject;

        public Renderer(string p_texture, GameObject obj)
        {
            texture = new Texture(p_texture);
            gameObject = obj;
        }

        public Renderer(Animation animation, GameObject obj)
        {
            currentAnimation = animation;
            gameObject = obj;
        }

        public Renderer(string p_texture, Animation animation, GameObject obj)
        {
            texture = new Texture(p_texture);
            currentAnimation = animation;
            gameObject = obj;
        }

        public int GetWidth()
        {
            if (currentAnimation != null)
            {
                return currentAnimation.CurrentFrame.Width;
            }

            return texture.Width;
        }

        public int GetHeight()
        {
            if (currentAnimation != null)
            {
                return currentAnimation.CurrentFrame.Height;
            }

            return texture.Height;
        }

        public void SetAnimation(Animation animation)
        {
            currentAnimation = animation;
        }

        public void Update()
        {
            currentAnimation.Update();
        }

        public void Draw(CTransform transform)
        {
            float drawPosX = transform.position.x - (gameObject.p_RealWidth / 2);
            float drawPosY = transform.position.y - (gameObject.p_RealHeight);


            if (currentAnimation == null)
            {
                try
                {
                    Engine.Draw(texture, drawPosX, drawPosY, transform.scale.x, transform.scale.y, transform.rotation.x, transform.rotation.y);
                }
                catch (System.Exception t)
                {
                    Console.WriteLine($"SE PRODUJO UN ERROR AL INTENTAR DIBUJAR UNA TEXTURA | ERROR: {t.Message}");
                }
            }
            else
            {
                Engine.Draw(currentAnimation.CurrentFrame, drawPosX, drawPosY, transform.scale.x, transform.scale.y, transform.rotation.x, transform.rotation.y);
            }
        }
    }

    public class CTransform
    {
        public Vector2 position;
        public Vector2 scale;
        public Vector2 rotation;

        public CTransform(float p_positionX = 0, float p_positionY = 0, float p_scaleX = 1, float p_scaleY = 1, float p_rotationX = 0, float p_rotationY = 0)
        {
            position = new Vector2(p_positionX, p_positionY);
            scale = new Vector2(p_scaleX, p_scaleY);
            rotation = new Vector2 (p_rotationX, p_rotationY);
        }
    }

    public class GameObject : ICollidable
    {
        protected Animation idle;

        protected Renderer renderer;

        protected CTransform cTransform;

        public float PosY => cTransform.position.y;
        public float PosX => cTransform.position.x;

        public CTransform p_cTransform => cTransform;

        protected int RealWidth => (int)(renderer.GetWidth() * cTransform.scale.x);
        protected int RealHeight => (int)(renderer.GetHeight() * cTransform.scale.y);

        public Vector2 RealSize => new Vector2(RealWidth, RealHeight);
        public Vector2 BottomCenterPosition => new Vector2(cTransform.position.x, cTransform.position.y);

        public int p_RealWidth => RealWidth;
        public int p_RealHeight => RealHeight;

        public GameObject(float p_sizeX, float p_sizeY, string p_textura, int p_posicionX, int p_posicionY)
        {
            cTransform = new CTransform(p_posicionX, p_posicionY, p_sizeX, p_sizeY);
            renderer = new Renderer(p_textura, this);

            GameUpdateManager.Instance.AddUpdatableObj(this);
        }

        protected void SetAnimation(Animation animation)
        {
            renderer.SetAnimation(animation);
        }

        public virtual void Input()
        {

        }

        public virtual void Update()
        {
            renderer.Update();
        }

        public virtual void Draw()
        {
            renderer.Draw(cTransform);
        }

        public virtual void OnCollision(GameObject other)
        {
            
        }
    }
}