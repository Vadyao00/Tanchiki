using OpenTK.Graphics.OpenGL4;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;
using StbImageSharp;
using System.IO;

namespace Libr
{
    /// <summary>
    /// Представляет текстуру, загруженную из файла.
    /// </summary>
    public class Texture
    {
        /// <summary>
        /// Идентификатор текстуры OpenGL.
        /// </summary>
        public readonly int Handle;
        /// <summary>
        /// Загружает текстуру из файла.
        /// </summary>
        /// <param name="path">Путь к файлу текстуры.</param>
        /// <returns>Объект текстуры.</returns>
        public static Texture LoadFromFile(string path)
        {
            int handle = GL.GenTexture();

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, handle);

            StbImage.stbi_set_flip_vertically_on_load(1);

            using (Stream stream = File.OpenRead(path))
            {
                ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return new Texture(handle);
        }
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Texture"/>.
        /// </summary>
        /// <param name="glHandle">Идентификатор текстуры OpenGL.</param>
        public Texture(int glHandle)
        {
            Handle = glHandle;
        }
    }
}
