using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game2048;

internal class GameContent
{
    public Texture2D EmptyTexture { get; }
    public Texture2D PreviousMoveButtonTexture { get; }
    public Texture2D NewGameButtonTexture { get; }
    public SpriteFont FontBig { get; }
    public SpriteFont FontMedium { get; }
    public SpriteFont FontSmall { get; }

    public GameContent(ContentManager contentManager, GraphicsDevice graphics)
    {
        EmptyTexture = new Texture2D(graphics, 1, 1, false, SurfaceFormat.Color);
        EmptyTexture.SetData(new[] { Color.White });
        PreviousMoveButtonTexture = contentManager.Load<Texture2D>($"bitmaps{Path.DirectorySeparatorChar}back");
        NewGameButtonTexture = contentManager.Load<Texture2D>($"bitmaps{Path.DirectorySeparatorChar}refresh");
        FontBig = contentManager.Load<SpriteFont>($"fonts{Path.DirectorySeparatorChar}FreeSansBoldBig");
        FontMedium = contentManager.Load<SpriteFont>($"fonts{Path.DirectorySeparatorChar}FreeSansBoldMedium");
        FontSmall = contentManager.Load<SpriteFont>($"fonts{Path.DirectorySeparatorChar}FreeSansBoldSmall");
    }
}
