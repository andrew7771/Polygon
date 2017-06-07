using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SharpGL
{
    /// <summary>
    /// Базовый класс для импорта команд, структур и констант OpenGL.
    /// Дополнительно реализует задачи инициализации / финализации OpenGL в 
    /// соответствии с текущими параметрами экрана. 
    /// Использование данного класса подразумевает создание производного класса,
    /// в котором по необходимости добавляются обработчики событий отрисовки
    /// и взаимодействия с устройствами ввода информации.
    /// </summary>
    [DefaultEvent("Paint")]
    [ToolboxItem(false)]
    public partial class GL : UserControl
    {
        public GL()
        {
            InitializeComponent();
        }
    }
}
