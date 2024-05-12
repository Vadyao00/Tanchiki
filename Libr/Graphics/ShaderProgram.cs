using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace Libr
{
    /// <summary>
    /// Представляет шейдерную программу, состоящую из вершинного и фрагментного шейдеров.
    /// </summary>
    public class ShaderProgram
    {
        private readonly int _vertexShader = 0;
        private readonly int _fragmentShader = 0;
        private readonly int _program = 0;
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ShaderProgram"/>.
        /// </summary>
        /// <param name="vertexfile">Путь к файлу вершинного шейдера.</param>
        /// <param name="fragmentfile">Путь к файлу фрагментного шейдера.</param>
        public ShaderProgram(string vertexfile, string fragmentfile)
        {
            _vertexShader = CreateShader(ShaderType.VertexShader, vertexfile);
            _fragmentShader = CreateShader(ShaderType.FragmentShader, fragmentfile);

            _program = GL.CreateProgram();
            GL.AttachShader(_program, _vertexShader);
            GL.AttachShader(_program, _fragmentShader);

            GL.LinkProgram(_program);
            GL.GetProgram(_program, GetProgramParameterName.LinkStatus, out var code);
            if (code != (int)All.True)
            {
                var infoLog = GL.GetProgramInfoLog(_program);
                throw new Exception($"Ошибка линковки шейдерной программы № {_program} \n\n {infoLog}");
            }

            DeleteShader(_vertexShader);
            DeleteShader(_fragmentShader);
        }
        /// <summary>
        /// Активирует шейдерную программу для использования.
        /// </summary>
        public void ActiveProgram() => GL.UseProgram(_program);
        /// <summary>
        /// Деактивирует текущую активную шейдерную программу.
        /// </summary>
        public void DeactiveProgram() => GL.UseProgram(0);
        /// <summary>
        /// Удаляет шейдерную программу.
        /// </summary>
        public void DeleteProgram() => GL.DeleteProgram(_program);
        /// <summary>
        /// Возвращает индекс атрибута программы по его имени.
        /// </summary>
        /// <param name="name">Имя атрибута.</param>
        /// <returns>Индекс атрибута программы.</returns>
        public int GetAttribProgram(string name) => GL.GetAttribLocation(_program, name);
        /// <summary>
        /// Создает шейдер указанного типа и компилирует его из указанного файла.
        /// </summary>
        /// <param name="shaderType">Тип шейдера.</param>
        /// <param name="shaderFile">Путь к файлу шейдера.</param>
        /// <returns>Идентификатор созданного шейдера.</returns>
        private int CreateShader(ShaderType shaderType, string shaderFile)
        {
            string shaderStr = File.ReadAllText(shaderFile);
            int shaderID = GL.CreateShader(shaderType);
            GL.ShaderSource(shaderID, shaderStr);
            GL.CompileShader(shaderID);

            GL.GetShader(shaderID, ShaderParameter.CompileStatus, out var code);
            if(code != (int)All.True)
            {
                var infoLog = GL.GetShaderInfoLog(shaderID);
                throw new Exception($"Ошибка компиляции шейдера № {shaderID} \n\n {infoLog}");
            }

            return shaderID;
        }
        /// <summary>
        /// Удаляет указанный шейдер.
        /// </summary>
        private void DeleteShader(int shader)
        {
            GL.DetachShader(_program, shader);
            GL.DeleteShader(shader);
        }
    }
}
