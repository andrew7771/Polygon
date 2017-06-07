﻿using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System;
using System.Text;
using System.Drawing;
namespace SharpGL
{
    partial class GL
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // GL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "GL";
            this.ResumeLayout(false);

        }

        #endregion

        const int WM_CREATE     = 0x0001;
        const int WM_DESTROY    = 0x0002;
        const int WM_ERASEBKGND = 0x0014;

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_CREATE      : StartOpenGL();        break;
                case WM_DESTROY     : StopOpenGL();         break;
                case WM_ERASEBKGND  : break;
                default             : base.WndProc(ref m);  break;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)  
        {
            if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv")
                base.OnPaintBackground(e);
        }

        [Browsable(false)]  
        public override System.Drawing.Image BackgroundImage
        {
            get { return base.BackgroundImage;  }
            set { base.BackgroundImage = value; }
        }
        
        [Browsable(false)]  
        public override ImageLayout BackgroundImageLayout
        {
            get { return base.BackgroundImageLayout;  }
            set { base.BackgroundImageLayout = value; }
        }

        [Browsable(false)]  
        public new BorderStyle BorderStyle {get; set;}

        Color backColor;
        Color foreColor;

        public override System.Drawing.Color ForeColor
        {
            get
            {
                return foreColor;
            }
            set
            {
                foreColor = value;
                if (ActivateOpenGL)
                    glColor4f(ForeColor.R / 255.0f, ForeColor.G / 255.0f, ForeColor.B / 255.0f, ForeColor.A / 255.0f);
            }
        }

        public override System.Drawing.Color BackColor
        {
            get
            {
                return backColor;
            }
            set
            {
                backColor = value;
                if (ActivateOpenGL)
                    glClearColor(BackColor.R / 255.0f, BackColor.G / 255.0f, BackColor.B / 255.0f, BackColor.A / 255.0f);
            }
        }

        protected uint hDC  { get; set; }
        protected uint RC   { get; set; }
        protected bool ActivateOpenGL { get; private set; }

        protected void StartOpenGL()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |    ControlStyles.ResizeRedraw, true);
            UpdateStyles();
            
            PIXELFORMATDESCRIPTOR pfd = new PIXELFORMATDESCRIPTOR();
            ZeroPixelDescriptor(ref pfd);
            pfd.nVersion = 1;
            pfd.dwFlags = (PFD_DRAW_TO_WINDOW | PFD_SUPPORT_OPENGL | PFD_DOUBLEBUFFER);
            pfd.iPixelType = (byte)(PFD_TYPE_RGBA);
            pfd.cColorBits = 32;
            pfd.cDepthBits = 32;
            pfd.iLayerType = (byte)(PFD_MAIN_PLANE);

            hDC = GetDC(Handle);

            int pixelFormat = 0;
            pixelFormat = ChoosePixelFormat(hDC, ref pfd);
            SetPixelFormat(hDC, pixelFormat, ref pfd);

            RC = wglCreateContext(hDC);
            wglMakeCurrent(hDC, RC);
            ActivateOpenGL = true;
            
            LoadFont();

            glClearColor(BackColor.R / 255.0f, BackColor.G / 255.0f, BackColor.B / 255.0f, BackColor.A / 255.0f);
            glColor4f(ForeColor.R / 255.0f, ForeColor.G / 255.0f, ForeColor.B / 255.0f, ForeColor.A / 255.0f);

        }

        private void StopOpenGL()
        {
            uint hDC = wglGetCurrentDC();
            uint RC = wglGetCurrentContext();
            ActivateOpenGL = false;
            wglMakeCurrent(0, 0);
            wglDeleteContext(RC);
            ReleaseDC(Handle, hDC);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            wglMakeCurrent(hDC, RC);
            base.OnPaint(e);
        }

        uint nFont;
        protected void LoadFont()
        {
            nFont = glGenLists(255);
            wglUseFontBitmaps(wglGetCurrentDC(), 0, 256, nFont);
        }

        public void OutText(string s, double x, double y, double z = 0)
        {
            // Create two different encodings.
            Encoding ascii = Encoding.GetEncoding(1251);
            Encoding unicode = Encoding.Unicode;

            // Convert the string into a byte[].
            byte[] unicodeBytes = unicode.GetBytes(s);

            // Perform the conversion from one encoding to the other.
            byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);

            glListBase(nFont);
            glRasterPos3d(x, y, z);
            glCallLists(s.Length, GL_UNSIGNED_BYTE, asciiBytes);
        }

        #region [ WinGL  ]
        /// <summary>
		/// return device context of the window
		/// </summary>
		/// <param name="hwnd">Handle to window</param>
		/// <returns></returns>
		[DllImport("user32", EntryPoint = "GetDC")]
		public static extern uint GetDC( IntPtr hWnd );

        [DllImport("user32", EntryPoint = "ReleaseDC")]
        public static extern int ReleaseDC(IntPtr hwnd, uint dc);

        [DllImport("opengl32", EntryPoint = "wglUseFontBitmaps")]
        public static extern int wglUseFontBitmaps(uint dc, uint first, uint count, uint ListBase);

        [StructLayout(LayoutKind.Sequential)] 
		public struct PIXELFORMATDESCRIPTOR 
		{
			public ushort  nSize; 
			public ushort  nVersion; 
			public uint    dwFlags; 
			public byte    iPixelType; 
			public byte    cColorBits; 
			public byte    cRedBits; 
			public byte    cRedShift; 
			public byte    cGreenBits; 
			public byte    cGreenShift; 
			public byte    cBlueBits; 
			public byte    cBlueShift; 
			public byte    cAlphaBits; 
			public byte    cAlphaShift; 
			public byte    cAccumBits; 
			public byte    cAccumRedBits; 
			public byte    cAccumGreenBits; 
			public byte    cAccumBlueBits; 
			public byte    cAccumAlphaBits; 
			public byte    cDepthBits; 
			public byte    cStencilBits; 
			public byte    cAuxBuffers; 
			public byte    iLayerType; 
			public byte    bReserved; 
			public uint    dwLayerMask; 
			public uint    dwVisibleMask; 
			public uint    dwDamageMask; 
			// 40 bytes total
		}

		public static void ZeroPixelDescriptor(ref PIXELFORMATDESCRIPTOR pfd)
		{
			pfd.nSize           = 40; // sizeof(PIXELFORMATDESCRIPTOR); 
			pfd.nVersion        = 0; 
			pfd.dwFlags         = 0;
			pfd.iPixelType      = 0;
			pfd.cColorBits      = 0; 
			pfd.cRedBits        = 0; 
			pfd.cRedShift       = 0; 
			pfd.cGreenBits      = 0; 
			pfd.cGreenShift     = 0; 
			pfd.cBlueBits       = 0; 
			pfd.cBlueShift      = 0; 
			pfd.cAlphaBits      = 0; 
			pfd.cAlphaShift     = 0; 
			pfd.cAccumBits      = 0; 
			pfd.cAccumRedBits   = 0; 
			pfd.cAccumGreenBits = 0;
			pfd.cAccumBlueBits  = 0; 
			pfd.cAccumAlphaBits = 0;
			pfd.cDepthBits      = 0; 
			pfd.cStencilBits    = 0; 
			pfd.cAuxBuffers     = 0; 
			pfd.iLayerType      = 0;
			pfd.bReserved       = 0; 
			pfd.dwLayerMask     = 0; 
			pfd.dwVisibleMask   = 0; 
			pfd.dwDamageMask    = 0; 
		}

		/* pixel types */
		public const uint  PFD_TYPE_RGBA        = 0;
		public const uint  PFD_TYPE_COLORINDEX  = 1;

		/* layer types */
		public const uint  PFD_MAIN_PLANE       = 0;
		public const uint  PFD_OVERLAY_PLANE    = 1;
		public const uint  PFD_UNDERLAY_PLANE   = 0xff; // (-1)

		/* PIXELFORMATDESCRIPTOR flags */
		public const uint  PFD_DOUBLEBUFFER            = 0x00000001;
		public const uint  PFD_STEREO                  = 0x00000002;
		public const uint  PFD_DRAW_TO_WINDOW          = 0x00000004;
		public const uint  PFD_DRAW_TO_BITMAP          = 0x00000008;
		public const uint  PFD_SUPPORT_GDI             = 0x00000010;
		public const uint  PFD_SUPPORT_OPENGL          = 0x00000020;
		public const uint  PFD_GENERIC_FORMAT          = 0x00000040;
		public const uint  PFD_NEED_PALETTE            = 0x00000080;
		public const uint  PFD_NEED_SYSTEM_PALETTE     = 0x00000100;
		public const uint  PFD_SWAP_EXCHANGE           = 0x00000200;
		public const uint  PFD_SWAP_COPY               = 0x00000400;
		public const uint  PFD_SWAP_LAYER_BUFFERS      = 0x00000800;
		public const uint  PFD_GENERIC_ACCELERATED     = 0x00001000;
		public const uint  PFD_SUPPORT_DIRECTDRAW      = 0x00002000;

		/* PIXELFORMATDESCRIPTOR flags for use in ChoosePixelFormat only */
		public const uint  PFD_DEPTH_DONTCARE          = 0x20000000;
		public const uint  PFD_DOUBLEBUFFER_DONTCARE   = 0x40000000;
		public const uint  PFD_STEREO_DONTCARE         = 0x80000000;

		/// <summary>
		/// Retrieves an index for a pixel format closest to what is passed
		/// </summary>
		/// <param name="hdc">Device context</param>
		/// <param name="p_pfd">Pixel Format Descriptor struct</param>
		/// <returns></returns>
		[DllImport("gdi32", EntryPoint = "ChoosePixelFormat")]
		public static extern int ChoosePixelFormat( uint hDC, ref PIXELFORMATDESCRIPTOR p_pfd );

		/// <summary>
		/// Sets the pixel format for the device context to the format specified by the index
		/// </summary>
		/// <param name="hdc">Device Context</param>
		/// <param name="iPixelFormat">Index to a pixel format returned ChoosePixelFormat</param>
		/// <param name="p_pfd">Pixel Format Descriptor</param>
		/// <returns></returns>
		[DllImport("gdi32", EntryPoint = "SetPixelFormat")]
		public static extern uint SetPixelFormat( uint hDC, int iPixelFormat, ref PIXELFORMATDESCRIPTOR p_pfd );

		/// <summary>
		/// Creates a rendering context for the Device context.
		/// </summary>
		/// <param name="hdc">Device Context</param>
		/// <returns></returns>
		[DllImport("opengl32", EntryPoint = "wglCreateContext")]
		public static extern uint wglCreateContext( uint hDC );

		/// <summary>
		/// Sets the current rendering context
		/// </summary>
		/// <param name="hdc">Device Context</param>
		/// <param name="hglrc">Rendering Context</param>
		/// <returns></returns>
		[DllImport("opengl32", EntryPoint = "wglMakeCurrent")]
		public static extern int wglMakeCurrent( uint hDC, uint hglrc );

		/// <summary>
		/// Deletes the rendering context
		/// </summary>
		/// <param name="hglrc">Rendering context to delet</param>
		/// <returns></returns>
		[DllImport("opengl32", EntryPoint = "wglDeleteContext")]
		public static extern int wglDeleteContext( uint hglrc );

		/// <summary>
		/// Swaps the display buffers in a double buffer context
		/// </summary>
		/// <param name="hdc">Device context</param>
		/// <returns></returns>
		[DllImport("opengl32", EntryPoint = "wglSwapBuffers")]
		public static extern uint wglSwapBuffers( uint hDC );

		[DllImport("opengl32", EntryPoint = "wglGetCurrentDC")]
        public static extern uint wglGetCurrentDC();

        [DllImport("opengl32", EntryPoint = "wglGetCurrentContext")]
        public static extern uint wglGetCurrentContext();

        #endregion

        #region [ OpenGL ]

		public const string GL_DLL="opengl32";

		/*Version*/
		public const uint   GL_VERSION_1_1=1;

		/*AccumOp*/
		public const uint   GL_ACCUM=0x0100;
		public const uint   GL_LOAD=0x0101;
		public const uint   GL_RETURN=0x0102;
		public const uint   GL_MULT=0x0103;
		public const uint   GL_ADD=0x0104;

		/*AlphaFunction*/
		public const uint   GL_NEVER=0x0200;
		public const uint   GL_LESS=0x0201;
		public const uint   GL_EQUAL=0x0202;
		public const uint   GL_LEQUAL=0x0203;
		public const uint   GL_GREATER=0x0204;
		public const uint   GL_NOTEQUAL=0x0205;
		public const uint   GL_GEQUAL=0x0206;
		public const uint   GL_ALWAYS=0x0207;

		/*AttribMask*/
		public const uint   GL_CURRENT_BIT=0x00000001;
		public const uint   GL_POINT_BIT=0x00000002;
		public const uint   GL_LINE_BIT=0x00000004;
		public const uint   GL_POLYGON_BIT=0x00000008;
		public const uint   GL_POLYGON_STIPPLE_BIT=0x00000010;
		public const uint   GL_PIXEL_MODE_BIT=0x00000020;
		public const uint   GL_LIGHTING_BIT=0x00000040;
		public const uint   GL_FOG_BIT=0x00000080;
		public const uint   GL_DEPTH_BUFFER_BIT=0x00000100;
		public const uint   GL_ACCUM_BUFFER_BIT=0x00000200;
		public const uint   GL_STENCIL_BUFFER_BIT=0x00000400;
		public const uint   GL_VIEWPORT_BIT=0x00000800;
		public const uint   GL_TRANSFORM_BIT=0x00001000;
		public const uint   GL_ENABLE_BIT=0x00002000;
		public const uint   GL_COLOR_BUFFER_BIT=0x00004000;
		public const uint   GL_HINT_BIT=0x00008000;
		public const uint   GL_EVAL_BIT=0x00010000;
		public const uint   GL_LIST_BIT=0x00020000;
		public const uint   GL_TEXTURE_BIT=0x00040000;
		public const uint   GL_SCISSOR_BIT=0x00080000;
		public const uint   GL_ALL_ATTRIB_BITS=0x000fffff;

		/*BeginMode*/
		public const uint   GL_POINTS=0x0000;
		public const uint   GL_LINES=0x0001;
		public const uint   GL_LINE_LOOP=0x0002;
		public const uint   GL_LINE_STRIP=0x0003;
		public const uint   GL_TRIANGLES=0x0004;
		public const uint   GL_TRIANGLE_STRIP=0x0005;
		public const uint   GL_TRIANGLE_FAN=0x0006;
		public const uint   GL_QUADS=0x0007;
		public const uint   GL_QUAD_STRIP=0x0008;
		public const uint   GL_POLYGON=0x0009;

		/*BlendingFactorDest*/
		public const uint   GL_ZERO=0;
		public const uint   GL_ONE=1;
		public const uint   GL_SRC_COLOR=0x0300;
		public const uint   GL_ONE_MINUS_SRC_COLOR=0x0301;
		public const uint   GL_SRC_ALPHA=0x0302;
		public const uint   GL_ONE_MINUS_SRC_ALPHA=0x0303;
		public const uint   GL_DST_ALPHA=0x0304;
		public const uint   GL_ONE_MINUS_DST_ALPHA=0x0305;

		/*BlendingFactorSrc*/
		/*GL_ZERO*/
		/*GL_ONE*/
		public const uint   GL_DST_COLOR=0x0306;
		public const uint   GL_ONE_MINUS_DST_COLOR=0x0307;
		public const uint   GL_SRC_ALPHA_SATURATE=0x0308;
		/*GL_SRC_ALPHA*/
		/*GL_ONE_MINUS_SRC_ALPHA*/
		/*GL_DST_ALPHA*/
		/*GL_ONE_MINUS_DST_ALPHA*/

		/*Boolean*/
		public const uint   GL_TRUE=1;
		public const uint   GL_FALSE=0;

		/*ClearBufferMask*/
		/*GL_COLOR_BUFFER_BIT*/
		/*GL_ACCUM_BUFFER_BIT*/
		/*GL_STENCIL_BUFFER_BIT*/
		/*GL_DEPTH_BUFFER_BIT*/

		/*ClientArrayType*/
		/*GL_VERTEX_ARRAY*/
		/*GL_NORMAL_ARRAY*/
		/*GL_COLOR_ARRAY*/
		/*GL_INDEX_ARRAY*/
		/*GL_TEXTURE_COORD_ARRAY*/
		/*GL_EDGE_FLAG_ARRAY*/

		/*ClipPlaneName*/
		public const uint   GL_CLIP_PLANE0=0x3000;
		public const uint   GL_CLIP_PLANE1=0x3001;
		public const uint   GL_CLIP_PLANE2=0x3002;
		public const uint   GL_CLIP_PLANE3=0x3003;
		public const uint   GL_CLIP_PLANE4=0x3004;
		public const uint   GL_CLIP_PLANE5=0x3005;

		/*ColorMaterialFace*/
		/*GL_FRONT*/
		/*GL_BACK*/
		/*GL_FRONT_AND_BACK*/

		/*ColorMaterialParameter*/
		/*GL_AMBIENT*/
		/*GL_DIFFUSE*/
		/*GL_SPECULAR*/
		/*GL_EMISSION*/
		/*GL_AMBIENT_AND_DIFFUSE*/

		/*ColorPointerType*/
		/*GL_byte */
		/*GL_UNSIGNED_byte */
		/*GL_SHORT*/
		/*GL_UNSIGNED_SHORT*/
		/*GL_int */
		/*GL_UNSIGNED_int */
		/*GL_float */
		/*GL_DOUBLE*/

		/*CullFaceMode*/
		/*GL_FRONT*/
		/*GL_BACK*/
		/*GL_FRONT_AND_BACK*/

		/*DataType*/
		public const uint   GL_BYTE =0x1400;
		public const uint   GL_UNSIGNED_BYTE =0x1401;
		public const uint   GL_SHORT=0x1402;
		public const uint   GL_UNSIGNED_SHORT=0x1403;
		public const uint   GL_INT =0x1404;
		public const uint   GL_UNSIGNED_INT =0x1405;
		public const uint   GL_FLOAT =0x1406;
		public const uint   GL_2_BYTES=0x1407;
		public const uint   GL_3_BYTES=0x1408;
		public const uint   GL_4_BYTES=0x1409;
		public const uint   GL_DOUBLE=0x140a;

		/*DepthFunction*/
		/*GL_NEVER*/
		/*GL_LESS*/
		/*GL_EQUAL*/
		/*GL_LEQUAL*/
		/*GL_GREATER*/
		/*GL_NOTEQUAL*/
		/*GL_GEQUAL*/
		/*GL_ALWAYS*/

		/*DrawBufferMode*/
		public const uint   GL_NONE=0;
		public const uint   GL_FRONT_LEFT=0x0400;
		public const uint   GL_FRONT_RIGHT=0x0401;
		public const uint   GL_BACK_LEFT=0x0402;
		public const uint   GL_BACK_RIGHT=0x0403;
		public const uint   GL_FRONT=0x0404;
		public const uint   GL_BACK=0x0405;
		public const uint   GL_LEFT=0x0406;
		public const uint   GL_RIGHT=0x0407;
		public const uint   GL_FRONT_AND_BACK=0x0408;
		public const uint   GL_AUX0=0x0409;
		public const uint   GL_AUX1=0x040a;
		public const uint   GL_AUX2=0x040b;
		public const uint   GL_AUX3=0x040c;

		/*Enable*/
		/*GL_FOG*/
		/*GL_LIGHTING*/
		/*GL_TEXTURE_1D*/
		/*GL_TEXTURE_2D*/
		/*GL_LINE_STIPPLE*/
		/*GL_POLYGON_STIPPLE*/
		/*GL_CULL_FACE*/
		/*GL_ALPHA_TEST*/
		/*GL_BLEND*/
		/*GL_INDEX_LOGIC_OP*/
		/*GL_COLOR_LOGIC_OP*/
		/*GL_DITHER*/
		/*GL_STENCIL_TEST*/
		/*GL_DEPTH_TEST*/
		/*GL_CLIP_PLANE0*/
		/*GL_CLIP_PLANE1*/
		/*GL_CLIP_PLANE2*/
		/*GL_CLIP_PLANE3*/
		/*GL_CLIP_PLANE4*/
		/*GL_CLIP_PLANE5*/
		/*GL_LIGHT0*/
		/*GL_LIGHT1*/
		/*GL_LIGHT2*/
		/*GL_LIGHT3*/
		/*GL_LIGHT4*/
		/*GL_LIGHT5*/
		/*GL_LIGHT6*/
		/*GL_LIGHT7*/
		/*GL_TEXTURE_GEN_S*/
		/*GL_TEXTURE_GEN_T*/
		/*GL_TEXTURE_GEN_R*/
		/*GL_TEXTURE_GEN_Q*/
		/*GL_MAP1_VERTEX_3*/
		/*GL_MAP1_VERTEX_4*/
		/*GL_MAP1_COLOR_4*/
		/*GL_MAP1_INDEX*/
		/*GL_MAP1_NORMAL*/
		/*GL_MAP1_TEXTURE_COORD_1*/
		/*GL_MAP1_TEXTURE_COORD_2*/
		/*GL_MAP1_TEXTURE_COORD_3*/
		/*GL_MAP1_TEXTURE_COORD_4*/
		/*GL_MAP2_VERTEX_3*/
		/*GL_MAP2_VERTEX_4*/
		/*GL_MAP2_COLOR_4*/
		/*GL_MAP2_INDEX*/
		/*GL_MAP2_NORMAL*/
		/*GL_MAP2_TEXTURE_COORD_1*/
		/*GL_MAP2_TEXTURE_COORD_2*/
		/*GL_MAP2_TEXTURE_COORD_3*/
		/*GL_MAP2_TEXTURE_COORD_4*/
		/*GL_POint _SMOOTH*/
		/*GL_LINE_SMOOTH*/
		/*GL_POLYGON_SMOOTH*/
		/*GL_SCISSOR_TEST*/
		/*GL_COLOR_MATERIAL*/
		/*GL_NORMALIZE*/
		/*GL_AUTO_NORMAL*/
		/*GL_VERTEX_ARRAY*/
		/*GL_NORMAL_ARRAY*/
		/*GL_COLOR_ARRAY*/
		/*GL_INDEX_ARRAY*/
		/*GL_TEXTURE_COORD_ARRAY*/
		/*GL_EDGE_FLAG_ARRAY*/
		/*GL_POLYGON_OFFSET_POint */
		/*GL_POLYGON_OFFSET_LINE*/
		/*GL_POLYGON_OFFSET_FILL*/

		/*ErrorCode*/
		public const uint   GL_NO_ERROR=0;
		public const uint   GL_INVALID_ENUM=0x0500;
		public const uint   GL_INVALID_VALUE=0x0501;
		public const uint   GL_INVALID_OPERATION=0x0502;
		public const uint   GL_STACK_OVERFLOW=0x0503;
		public const uint   GL_STACK_UNDERFLOW=0x0504;
		public const uint   GL_OUT_OF_MEMORY=0x0505;

		/*FeedBackMode*/
		public const uint   GL_2D=0x0600;
		public const uint   GL_3D=0x0601;
		public const uint   GL_3D_COLOR=0x0602;
		public const uint   GL_3D_COLOR_TEXTURE=0x0603;
		public const uint   GL_4D_COLOR_TEXTURE=0x0604;

		/*FeedBackToken*/
		public const uint   GL_PASS_THROUGH_TOKEN=0x0700;
		public const uint   GL_POINT_TOKEN=0x0701;
		public const uint   GL_LINE_TOKEN=0x0702;
		public const uint   GL_POLYGON_TOKEN=0x0703;
		public const uint   GL_BITMAP_TOKEN=0x0704;
		public const uint   GL_DRAW_PIXEL_TOKEN=0x0705;
		public const uint   GL_COPY_PIXEL_TOKEN=0x0706;
		public const uint   GL_LINE_RESET_TOKEN=0x0707;

		/*FogMode*/
		/*GL_LINEAR*/
		public const uint   GL_EXP=0x0800;
		public const uint   GL_EXP2=0x0801;

		/*FogParameter*/
		/*GL_FOG_COLOR*/
		/*GL_FOG_DENSITY*/
		/*GL_FOG_END*/
		/*GL_FOG_INDEX*/
		/*GL_FOG_MODE*/
		/*GL_FOG_START*/

		/*FrontFaceDirection*/
		public const uint   GL_CW=0x0900;
		public const uint   GL_CCW=0x0901;

		/*GetMapTarget*/
		public const uint   GL_COEFF=0x0a00;
		public const uint   GL_ORDER=0x0a01;
		public const uint   GL_DOMAIN=0x0a02;

		/*GetPixelMap*/
		/*GL_PIXEL_MAP_I_TO_I*/
		/*GL_PIXEL_MAP_S_TO_S*/
		/*GL_PIXEL_MAP_I_TO_R*/
		/*GL_PIXEL_MAP_I_TO_G*/
		/*GL_PIXEL_MAP_I_TO_B*/
		/*GL_PIXEL_MAP_I_TO_A*/
		/*GL_PIXEL_MAP_R_TO_R*/
		/*GL_PIXEL_MAP_G_TO_G*/
		/*GL_PIXEL_MAP_B_TO_B*/
		/*GL_PIXEL_MAP_A_TO_A*/

		/*GetPointerTarget*/
		/*GL_VERTEX_ARRAY_POint ER*/
		/*GL_NORMAL_ARRAY_POint ER*/
		/*GL_COLOR_ARRAY_POint ER*/
		/*GL_INDEX_ARRAY_POint ER*/
		/*GL_TEXTURE_COORD_ARRAY_POint ER*/
		/*GL_EDGE_FLAG_ARRAY_POint ER*/

		/*GetTarget*/
		public const uint   GL_CURRENT_COLOR=0x0b00;
		public const uint   GL_CURRENT_INDEX=0x0b01;
		public const uint   GL_CURRENT_NORMAL=0x0b02;
		public const uint   GL_CURRENT_TEXTURE_COORDS=0x0b03;
		public const uint   GL_CURRENT_RASTER_COLOR=0x0b04;
		public const uint   GL_CURRENT_RASTER_INDEX=0x0b05;
		public const uint   GL_CURRENT_RASTER_TEXTURE_COORDS=0x0b06;
		public const uint   GL_CURRENT_RASTER_POSITION=0x0b07;
		public const uint   GL_CURRENT_RASTER_POSITION_VALID=0x0b08;
		public const uint   GL_CURRENT_RASTER_DISTANCE=0x0b09;
		public const uint   GL_POINT_SMOOTH=0x0b10;
		public const uint   GL_POINT_SIZE=0x0b11;
		public const uint   GL_POINT_SIZE_RANGE=0x0b12;
		public const uint   GL_POINT_SIZE_GRANULARITY=0x0b13;
		public const uint   GL_LINE_SMOOTH=0x0b20;
		public const uint   GL_LINE_WIDTH=0x0b21;
		public const uint   GL_LINE_WIDTH_RANGE=0x0b22;
		public const uint   GL_LINE_WIDTH_GRANULARITY=0x0b23;
		public const uint   GL_LINE_STIPPLE=0x0b24;
		public const uint   GL_LINE_STIPPLE_PATTERN=0x0b25;
		public const uint   GL_LINE_STIPPLE_REPEAT=0x0b26;
		public const uint   GL_LIST_MODE=0x0b30;
		public const uint   GL_MAX_LIST_NESTING=0x0b31;
		public const uint   GL_LIST_BASE=0x0b32;
		public const uint   GL_LIST_INDEX=0x0b33;
		public const uint   GL_POLYGON_MODE=0x0b40;
		public const uint   GL_POLYGON_SMOOTH=0x0b41;
		public const uint   GL_POLYGON_STIPPLE=0x0b42;
		public const uint   GL_EDGE_FLAG=0x0b43;
		public const uint   GL_CULL_FACE=0x0b44;
		public const uint   GL_CULL_FACE_MODE=0x0b45;
		public const uint   GL_FRONT_FACE=0x0b46;
		public const uint   GL_LIGHTING=0x0b50;
		public const uint   GL_LIGHT_MODEL_LOCAL_VIEWER=0x0b51;
		public const uint   GL_LIGHT_MODEL_TWO_SIDE=0x0b52;
		public const uint   GL_LIGHT_MODEL_AMBIENT=0x0b53;
		public const uint   GL_SHADE_MODEL=0x0b54;
		public const uint   GL_COLOR_MATERIAL_FACE=0x0b55;
		public const uint   GL_COLOR_MATERIAL_PARAMETER=0x0b56;
		public const uint   GL_COLOR_MATERIAL=0x0b57;
		public const uint   GL_FOG=0x0b60;
		public const uint   GL_FOG_INDEX=0x0b61;
		public const uint   GL_FOG_DENSITY=0x0b62;
		public const uint   GL_FOG_START=0x0b63;
		public const uint   GL_FOG_END=0x0b64;
		public const uint   GL_FOG_MODE=0x0b65;
		public const uint   GL_FOG_COLOR=0x0b66;
		public const uint   GL_DEPTH_RANGE=0x0b70;
		public const uint   GL_DEPTH_TEST=0x0b71;
		public const uint   GL_DEPTH_WRITEMASK=0x0b72;
		public const uint   GL_DEPTH_CLEAR_VALUE=0x0b73;
		public const uint   GL_DEPTH_FUNC=0x0b74;
		public const uint   GL_ACCUM_CLEAR_VALUE=0x0b80;
		public const uint   GL_STENCIL_TEST=0x0b90;
		public const uint   GL_STENCIL_CLEAR_VALUE=0x0b91;
		public const uint   GL_STENCIL_FUNC=0x0b92;
		public const uint   GL_STENCIL_VALUE_MASK=0x0b93;
		public const uint   GL_STENCIL_FAIL=0x0b94;
		public const uint   GL_STENCIL_PASS_DEPTH_FAIL=0x0b95;
		public const uint   GL_STENCIL_PASS_DEPTH_PASS=0x0b96;
		public const uint   GL_STENCIL_REF=0x0b97;
		public const uint   GL_STENCIL_WRITEMASK=0x0b98;
		public const uint   GL_MATRIX_MODE=0x0ba0;
		public const uint   GL_NORMALIZE=0x0ba1;
		public const uint   GL_VIEWPORT=0x0ba2;
		public const uint   GL_MODELVIEW_STACK_DEPTH=0x0ba3;
		public const uint   GL_PROJECTION_STACK_DEPTH=0x0ba4;
		public const uint   GL_TEXTURE_STACK_DEPTH=0x0ba5;
		public const uint   GL_MODELVIEW_MATRIX=0x0ba6;
		public const uint   GL_PROJECTION_MATRIX=0x0ba7;
		public const uint   GL_TEXTURE_MATRIX=0x0ba8;
		public const uint   GL_ATTRIB_STACK_DEPTH=0x0bb0;
		public const uint   GL_CLIENT_ATTRIB_STACK_DEPTH=0x0bb1;
		public const uint   GL_ALPHA_TEST=0x0bc0;
		public const uint   GL_ALPHA_TEST_FUNC=0x0bc1;
		public const uint   GL_ALPHA_TEST_REF=0x0bc2;
		public const uint   GL_DITHER=0x0bd0;
		public const uint   GL_BLEND_DST=0x0be0;
		public const uint   GL_BLEND_SRC=0x0be1;
		public const uint   GL_BLEND=0x0be2;
		public const uint   GL_LOGIC_OP_MODE=0x0bf0;
		public const uint   GL_INDEX_LOGIC_OP=0x0bf1;
		public const uint   GL_COLOR_LOGIC_OP=0x0bf2;
		public const uint   GL_AUX_BUFFERS=0x0c00;
		public const uint   GL_DRAW_BUFFER=0x0c01;
		public const uint   GL_READ_BUFFER=0x0c02;
		public const uint   GL_SCISSOR_BOX=0x0c10;
		public const uint   GL_SCISSOR_TEST=0x0c11;
		public const uint   GL_INDEX_CLEAR_VALUE=0x0c20;
		public const uint   GL_INDEX_WRITEMASK=0x0c21;
		public const uint   GL_COLOR_CLEAR_VALUE=0x0c22;
		public const uint   GL_COLOR_WRITEMASK=0x0c23;
		public const uint   GL_INDEX_MODE=0x0c30;
		public const uint   GL_RGBA_MODE=0x0c31;
		public const uint   GL_DOUBLEBUFFER=0x0c32;
		public const uint   GL_STEREO=0x0c33;
		public const uint   GL_RENDER_MODE=0x0c40;
		public const uint   GL_PERSPECTIVE_CORRECTION_Hint =0x0c50;
		public const uint   GL_POINT_SMOOTH_Hint =0x0c51;
		public const uint   GL_LINE_SMOOTH_Hint =0x0c52;
		public const uint   GL_POLYGON_SMOOTH_Hint =0x0c53;
		public const uint   GL_FOG_Hint =0x0c54;
		public const uint   GL_TEXTURE_GEN_S=0x0c60;
		public const uint   GL_TEXTURE_GEN_T=0x0c61;
		public const uint   GL_TEXTURE_GEN_R=0x0c62;
		public const uint   GL_TEXTURE_GEN_Q=0x0c63;
		public const uint   GL_PIXEL_MAP_I_TO_I=0x0c70;
		public const uint   GL_PIXEL_MAP_S_TO_S=0x0c71;
		public const uint   GL_PIXEL_MAP_I_TO_R=0x0c72;
		public const uint   GL_PIXEL_MAP_I_TO_G=0x0c73;
		public const uint   GL_PIXEL_MAP_I_TO_B=0x0c74;
		public const uint   GL_PIXEL_MAP_I_TO_A=0x0c75;
		public const uint   GL_PIXEL_MAP_R_TO_R=0x0c76;
		public const uint   GL_PIXEL_MAP_G_TO_G=0x0c77;
		public const uint   GL_PIXEL_MAP_B_TO_B=0x0c78;
		public const uint   GL_PIXEL_MAP_A_TO_A=0x0c79;
		public const uint   GL_PIXEL_MAP_I_TO_I_SIZE=0x0cb0;
		public const uint   GL_PIXEL_MAP_S_TO_S_SIZE=0x0cb1;
		public const uint   GL_PIXEL_MAP_I_TO_R_SIZE=0x0cb2;
		public const uint   GL_PIXEL_MAP_I_TO_G_SIZE=0x0cb3;
		public const uint   GL_PIXEL_MAP_I_TO_B_SIZE=0x0cb4;
		public const uint   GL_PIXEL_MAP_I_TO_A_SIZE=0x0cb5;
		public const uint   GL_PIXEL_MAP_R_TO_R_SIZE=0x0cb6;
		public const uint   GL_PIXEL_MAP_G_TO_G_SIZE=0x0cb7;
		public const uint   GL_PIXEL_MAP_B_TO_B_SIZE=0x0cb8;
		public const uint   GL_PIXEL_MAP_A_TO_A_SIZE=0x0cb9;
		public const uint   GL_UNPACK_SWAP_BYTES=0x0cf0;
		public const uint   GL_UNPACK_LSB_FIRST=0x0cf1;
		public const uint   GL_UNPACK_ROW_LENGTH=0x0cf2;
		public const uint   GL_UNPACK_SKIP_ROWS=0x0cf3;
		public const uint   GL_UNPACK_SKIP_PIXELS=0x0cf4;
		public const uint   GL_UNPACK_ALIGNMENT=0x0cf5;
		public const uint   GL_PACK_SWAP_BYTES=0x0d00;
		public const uint   GL_PACK_LSB_FIRST=0x0d01;
		public const uint   GL_PACK_ROW_LENGTH=0x0d02;
		public const uint   GL_PACK_SKIP_ROWS=0x0d03;
		public const uint   GL_PACK_SKIP_PIXELS=0x0d04;
		public const uint   GL_PACK_ALIGNMENT=0x0d05;
		public const uint   GL_MAP_COLOR=0x0d10;
		public const uint   GL_MAP_STENCIL=0x0d11;
		public const uint   GL_INDEX_SHIFT=0x0d12;
		public const uint   GL_INDEX_OFFSET=0x0d13;
		public const uint   GL_RED_SCALE=0x0d14;
		public const uint   GL_RED_BIAS=0x0d15;
		public const uint   GL_ZOOM_X=0x0d16;
		public const uint   GL_ZOOM_Y=0x0d17;
		public const uint   GL_GREEN_SCALE=0x0d18;
		public const uint   GL_GREEN_BIAS=0x0d19;
		public const uint   GL_BLUE_SCALE=0x0d1a;
		public const uint   GL_BLUE_BIAS=0x0d1b;
		public const uint   GL_ALPHA_SCALE=0x0d1c;
		public const uint   GL_ALPHA_BIAS=0x0d1d;
		public const uint   GL_DEPTH_SCALE=0x0d1e;
		public const uint   GL_DEPTH_BIAS=0x0d1f;
		public const uint   GL_MAX_EVAL_ORDER=0x0d30;
		public const uint   GL_MAX_LIGHTS=0x0d31;
		public const uint   GL_MAX_CLIP_PLANES=0x0d32;
		public const uint   GL_MAX_TEXTURE_SIZE=0x0d33;
		public const uint   GL_MAX_PIXEL_MAP_TABLE=0x0d34;
		public const uint   GL_MAX_ATTRIB_STACK_DEPTH=0x0d35;
		public const uint   GL_MAX_MODELVIEW_STACK_DEPTH=0x0d36;
		public const uint   GL_MAX_NAME_STACK_DEPTH=0x0d37;
		public const uint   GL_MAX_PROJECTION_STACK_DEPTH=0x0d38;
		public const uint   GL_MAX_TEXTURE_STACK_DEPTH=0x0d39;
		public const uint   GL_MAX_VIEWPORT_DIMS=0x0d3a;
		public const uint   GL_MAX_CLIENT_ATTRIB_STACK_DEPTH=0x0d3b;
		public const uint   GL_SUBPIXEL_BITS=0x0d50;
		public const uint   GL_INDEX_BITS=0x0d51;
		public const uint   GL_RED_BITS=0x0d52;
		public const uint   GL_GREEN_BITS=0x0d53;
		public const uint   GL_BLUE_BITS=0x0d54;
		public const uint   GL_ALPHA_BITS=0x0d55;
		public const uint   GL_DEPTH_BITS=0x0d56;
		public const uint   GL_STENCIL_BITS=0x0d57;
		public const uint   GL_ACCUM_RED_BITS=0x0d58;
		public const uint   GL_ACCUM_GREEN_BITS=0x0d59;
		public const uint   GL_ACCUM_BLUE_BITS=0x0d5a;
		public const uint   GL_ACCUM_ALPHA_BITS=0x0d5b;
		public const uint   GL_NAME_STACK_DEPTH=0x0d70;
		public const uint   GL_AUTO_NORMAL=0x0d80;
		public const uint   GL_MAP1_COLOR_4=0x0d90;
		public const uint   GL_MAP1_INDEX=0x0d91;
		public const uint   GL_MAP1_NORMAL=0x0d92;
		public const uint   GL_MAP1_TEXTURE_COORD_1=0x0d93;
		public const uint   GL_MAP1_TEXTURE_COORD_2=0x0d94;
		public const uint   GL_MAP1_TEXTURE_COORD_3=0x0d95;
		public const uint   GL_MAP1_TEXTURE_COORD_4=0x0d96;
		public const uint   GL_MAP1_VERTEX_3=0x0d97;
		public const uint   GL_MAP1_VERTEX_4=0x0d98;
		public const uint   GL_MAP2_COLOR_4=0x0db0;
		public const uint   GL_MAP2_INDEX=0x0db1;
		public const uint   GL_MAP2_NORMAL=0x0db2;
		public const uint   GL_MAP2_TEXTURE_COORD_1=0x0db3;
		public const uint   GL_MAP2_TEXTURE_COORD_2=0x0db4;
		public const uint   GL_MAP2_TEXTURE_COORD_3=0x0db5;
		public const uint   GL_MAP2_TEXTURE_COORD_4=0x0db6;
		public const uint   GL_MAP2_VERTEX_3=0x0db7;
		public const uint   GL_MAP2_VERTEX_4=0x0db8;
		public const uint   GL_MAP1_GRID_DOMAIN=0x0dd0;
		public const uint   GL_MAP1_GRID_SEGMENTS=0x0dd1;
		public const uint   GL_MAP2_GRID_DOMAIN=0x0dd2;
		public const uint   GL_MAP2_GRID_SEGMENTS=0x0dd3;
		public const uint   GL_TEXTURE_1D=0x0de0;
		public const uint   GL_TEXTURE_2D=0x0de1;
		public const uint   GL_FEEDBACK_BUFFER_POINTER=0x0df0;
		public const uint   GL_FEEDBACK_BUFFER_SIZE=0x0df1;
		public const uint   GL_FEEDBACK_BUFFER_TYPE=0x0df2;
		public const uint   GL_SELECTION_BUFFER_POINTER=0x0df3;
		public const uint   GL_SELECTION_BUFFER_SIZE=0x0df4;
		/*GL_TEXTURE_BINDING_1D*/
		/*GL_TEXTURE_BINDING_2D*/
		/*GL_VERTEX_ARRAY*/
		/*GL_NORMAL_ARRAY*/
		/*GL_COLOR_ARRAY*/
		/*GL_INDEX_ARRAY*/
		/*GL_TEXTURE_COORD_ARRAY*/
		/*GL_EDGE_FLAG_ARRAY*/
		/*GL_VERTEX_ARRAY_SIZE*/
		/*GL_VERTEX_ARRAY_TYPE*/
		/*GL_VERTEX_ARRAY_STRIDE*/
		/*GL_NORMAL_ARRAY_TYPE*/
		/*GL_NORMAL_ARRAY_STRIDE*/
		/*GL_COLOR_ARRAY_SIZE*/
		/*GL_COLOR_ARRAY_TYPE*/
		/*GL_COLOR_ARRAY_STRIDE*/
		/*GL_INDEX_ARRAY_TYPE*/
		/*GL_INDEX_ARRAY_STRIDE*/
		/*GL_TEXTURE_COORD_ARRAY_SIZE*/
		/*GL_TEXTURE_COORD_ARRAY_TYPE*/
		/*GL_TEXTURE_COORD_ARRAY_STRIDE*/
		/*GL_EDGE_FLAG_ARRAY_STRIDE*/
		/*GL_POLYGON_OFFSET_FACTOR*/
		/*GL_POLYGON_OFFSET_UNITS*/

		/*GetTextureParameter*/
		/*GL_TEXTURE_MAG_FILTER*/
		/*GL_TEXTURE_MIN_FILTER*/
		/*GL_TEXTURE_WRAP_S*/
		/*GL_TEXTURE_WRAP_T*/
		public const uint   GL_TEXTURE_WIDTH=0x1000;
		public const uint   GL_TEXTURE_HEIGHT=0x1001;
		public const uint   GL_TEXTURE_INTERNAL_FORMAT=0x1003;
		public const uint   GL_TEXTURE_BORDER_COLOR=0x1004;
		public const uint   GL_TEXTURE_BORDER=0x1005;
		/*GL_TEXTURE_RED_SIZE*/
		/*GL_TEXTURE_GREEN_SIZE*/
		/*GL_TEXTURE_BLUE_SIZE*/
		/*GL_TEXTURE_ALPHA_SIZE*/
		/*GL_TEXTURE_LUMINANCE_SIZE*/
		/*GL_TEXTURE_int ENSITY_SIZE*/
		/*GL_TEXTURE_PRIORITY*/
		/*GL_TEXTURE_RESIDENT*/

		/*Hint Mode*/
		public const uint   GL_DONT_CARE=0x1100;
		public const uint   GL_FASTEST=0x1101;
		public const uint   GL_NICEST=0x1102;

		/*Hint Target*/
		/*GL_PERSPECTIVE_CORRECTION_Hint */
		/*GL_POINT_SMOOTH_HINT */
		/*GL_LINE_SMOOTH_HINT */
		/*GL_POLYGON_SMOOTH_HINT */
		/*GL_FOG_HINT */
		/*GL_PHONG_HINT */

		/*Index Pointer Type*/
		/*GL_SHORT*/
		/*GL_INT */
		/*GL_FLOAT */
		/*GL_DOUBLE*/

		/*Light Model Parameter*/
		/*GL_LIGHT_MODEL_AMBIENT*/
		/*GL_LIGHT_MODEL_LOCAL_VIEWER*/
		/*GL_LIGHT_MODEL_TWO_SIDE*/

		/*Light Name*/
		public const uint   GL_LIGHT0=0x4000;
		public const uint   GL_LIGHT1=0x4001;
		public const uint   GL_LIGHT2=0x4002;
		public const uint   GL_LIGHT3=0x4003;
		public const uint   GL_LIGHT4=0x4004;
		public const uint   GL_LIGHT5=0x4005;
		public const uint   GL_LIGHT6=0x4006;
		public const uint   GL_LIGHT7=0x4007;

		/*LightParameter*/
		public const uint   GL_AMBIENT=0x1200;
		public const uint   GL_DIFFUSE=0x1201;
		public const uint   GL_SPECULAR=0x1202;
		public const uint   GL_POSITION=0x1203;
		public const uint   GL_SPOT_DIRECTION=0x1204;
		public const uint   GL_SPOT_EXPONENT=0x1205;
		public const uint   GL_SPOT_CUTOFF=0x1206;
		public const uint   GL_CONSTANT_ATTENUATION=0x1207;
		public const uint   GL_LINEAR_ATTENUATION=0x1208;
		public const uint   GL_QUADRATIC_ATTENUATION=0x1209;

		/*Interleaved Arrays*/
		/*GL_V2F*/
		/*GL_V3F*/
		/*GL_C4UB_V2F*/
		/*GL_C4UB_V3F*/
		/*GL_C3F_V3F*/
		/*GL_N3F_V3F*/
		/*GL_C4F_N3F_V3F*/
		/*GL_T2F_V3F*/
		/*GL_T4F_V4F*/
		/*GL_T2F_C4UB_V3F*/
		/*GL_T2F_C3F_V3F*/
		/*GL_T2F_N3F_V3F*/
		/*GL_T2F_C4F_N3F_V3F*/
		/*GL_T4F_C4F_N3F_V4F*/

		/*ListMode*/
		public const uint   GL_COMPILE=0x1300;
		public const uint   GL_COMPILE_AND_EXECUTE=0x1301;

		/*ListNameType*/
		/*GL_BYTE */
		/*GL_UNSIGNED_byte */
		/*GL_SHORT*/
		/*GL_UNSIGNED_SHORT*/
		/*GL_INT */
		/*GL_UNSIGNED_int */
		/*GL_FLOAT */
		/*GL_2_BYTES*/
		/*GL_3_BYTES*/
		/*GL_4_BYTES*/

		/*LogicOp*/
		public const uint   GL_CLEAR=0x1500;
		public const uint   GL_AND=0x1501;
		public const uint   GL_AND_REVERSE=0x1502;
		public const uint   GL_COPY=0x1503;
		public const uint   GL_AND_INVERTED=0x1504;
		public const uint   GL_NOOP=0x1505;
		public const uint   GL_XOR=0x1506;
		public const uint   GL_OR=0x1507;
		public const uint   GL_NOR=0x1508;
		public const uint   GL_EQUIV=0x1509;
		public const uint   GL_INVERT=0x150a;
		public const uint   GL_OR_REVERSE=0x150b;
		public const uint   GL_COPY_INVERTED=0x150c;
		public const uint   GL_OR_INVERTED=0x150d;
		public const uint   GL_NAND=0x150e;
		public const uint   GL_SET=0x150f;

		/*MapTarget*/
		/*GL_MAP1_COLOR_4*/
		/*GL_MAP1_INDEX*/
		/*GL_MAP1_NORMAL*/
		/*GL_MAP1_TEXTURE_COORD_1*/
		/*GL_MAP1_TEXTURE_COORD_2*/
		/*GL_MAP1_TEXTURE_COORD_3*/
		/*GL_MAP1_TEXTURE_COORD_4*/
		/*GL_MAP1_VERTEX_3*/
		/*GL_MAP1_VERTEX_4*/
		/*GL_MAP2_COLOR_4*/
		/*GL_MAP2_INDEX*/
		/*GL_MAP2_NORMAL*/
		/*GL_MAP2_TEXTURE_COORD_1*/
		/*GL_MAP2_TEXTURE_COORD_2*/
		/*GL_MAP2_TEXTURE_COORD_3*/
		/*GL_MAP2_TEXTURE_COORD_4*/
		/*GL_MAP2_VERTEX_3*/
		/*GL_MAP2_VERTEX_4*/

		/*MaterialFace*/
		/*GL_FRONT*/
		/*GL_BACK*/
		/*GL_FRONT_AND_BACK*/

		/*MaterialParameter*/
		public const uint   GL_EMISSION=0x1600;
		public const uint   GL_SHININESS=0x1601;
		public const uint   GL_AMBIENT_AND_DIFFUSE=0x1602;
		public const uint   GL_COLOR_INDEXES=0x1603;
		/*GL_AMBIENT*/
		/*GL_DIFFUSE*/
		/*GL_SPECULAR*/

		/*MatrixMode*/
		public const uint   GL_MODELVIEW=0x1700;
		public const uint   GL_PROJECTION=0x1701;
		public const uint   GL_TEXTURE=0x1702;

		/*MeshMode1*/
		/*GL_POINT */
		/*GL_LINE*/

		/*MeshMode2*/
		/*GL_POINT */
		/*GL_LINE*/
		/*GL_FILL*/

		/*NormalPoint erType*/
		/*GL_BYTE */
		/*GL_SHORT*/
		/*GL_INT */
		/*GL_FLOAT */
		/*GL_DOUBLE*/

		/*PixelCopyType*/
		public const uint   GL_COLOR=0x1800;
		public const uint   GL_DEPTH=0x1801;
		public const uint   GL_STENCIL=0x1802;

		/*PixelFormat*/
		public const uint   GL_COLOR_INDEX=0x1900;
		public const uint   GL_STENCIL_INDEX=0x1901;
		public const uint   GL_DEPTH_COMPONENT=0x1902;
		public const uint   GL_RED=0x1903;
		public const uint   GL_GREEN=0x1904;
		public const uint   GL_BLUE=0x1905;
		public const uint   GL_ALPHA=0x1906;
		public const uint   GL_RGB=0x1907;
		public const uint   GL_RGBA=0x1908;
		public const uint   GL_LUMINANCE=0x1909;
		public const uint   GL_LUMINANCE_ALPHA=0x190a;

		/*PixelMap*/
		/*GL_PIXEL_MAP_I_TO_I*/
		/*GL_PIXEL_MAP_S_TO_S*/
		/*GL_PIXEL_MAP_I_TO_R*/
		/*GL_PIXEL_MAP_I_TO_G*/
		/*GL_PIXEL_MAP_I_TO_B*/
		/*GL_PIXEL_MAP_I_TO_A*/
		/*GL_PIXEL_MAP_R_TO_R*/
		/*GL_PIXEL_MAP_G_TO_G*/
		/*GL_PIXEL_MAP_B_TO_B*/
		/*GL_PIXEL_MAP_A_TO_A*/

		/*PixelStore*/
		/*GL_UNPACK_SWAP_byte S*/
		/*GL_UNPACK_LSB_FIRST*/
		/*GL_UNPACK_ROW_LENGTH*/
		/*GL_UNPACK_SKIP_ROWS*/
		/*GL_UNPACK_SKIP_PIXELS*/
		/*GL_UNPACK_ALIGNMENT*/
		/*GL_PACK_SWAP_byte S*/
		/*GL_PACK_LSB_FIRST*/
		/*GL_PACK_ROW_LENGTH*/
		/*GL_PACK_SKIP_ROWS*/
		/*GL_PACK_SKIP_PIXELS*/
		/*GL_PACK_ALIGNMENT*/

		/*PixelTransfer*/
		/*GL_MAP_COLOR*/
		/*GL_MAP_STENCIL*/
		/*GL_INDEX_SHIFT*/
		/*GL_INDEX_OFFSET*/
		/*GL_RED_SCALE*/
		/*GL_RED_BIAS*/
		/*GL_GREEN_SCALE*/
		/*GL_GREEN_BIAS*/
		/*GL_BLUE_SCALE*/
		/*GL_BLUE_BIAS*/
		/*GL_ALPHA_SCALE*/
		/*GL_ALPHA_BIAS*/
		/*GL_DEPTH_SCALE*/
		/*GL_DEPTH_BIAS*/

		/*PixelType*/
		public const uint   GL_BITMAP=0x1a00;
		/*GL_BYTE */
		/*GL_UNSIGNED_byte */
		/*GL_SHORT*/
		/*GL_UNSIGNED_SHORT*/
		/*GL_INT*/
		/*GL_UNSIGNED_int */
		/*GL_FLOAT*/

		/*PolygonMode*/
		public const uint   GL_POINT=0x1b00;
		public const uint   GL_LINE=0x1b01;
		public const uint   GL_FILL=0x1b02;

		/*ReadBufferMode*/
		/*GL_FRONT_LEFT*/
		/*GL_FRONT_RIGHT*/
		/*GL_BACK_LEFT*/
		/*GL_BACK_RIGHT*/
		/*GL_FRONT*/
		/*GL_BACK*/
		/*GL_LEFT*/
		/*GL_RIGHT*/
		/*GL_AUX0*/
		/*GL_AUX1*/
		/*GL_AUX2*/
		/*GL_AUX3*/

		/*RenderingMode*/
		public const uint   GL_RENDER=0x1c00;
		public const uint   GL_FEEDBACK=0x1c01;
		public const uint   GL_SELECT=0x1c02;

		/*ShadingModel*/
		public const uint   GL_FLAT=0x1d00;
		public const uint   GL_SMOOTH=0x1d01;

		/*StencilFunction*/
		/*GL_NEVER*/
		/*GL_LESS*/
		/*GL_EQUAL*/
		/*GL_LEQUAL*/
		/*GL_GREATER*/
		/*GL_NOTEQUAL*/
		/*GL_GEQUAL*/
		/*GL_ALWAYS*/

		/*StencilOp*/
		/*GL_ZERO*/
		public const uint   GL_KEEP=0x1e00;
		public const uint   GL_REPLACE=0x1e01;
		public const uint   GL_INCR=0x1e02;
		public const uint   GL_DECR=0x1e03;
		/*GL_INVERT*/

		/*StringName*/
		public const uint   GL_VENDOR=0x1f00;
		public const uint   GL_RENDERER=0x1f01;
		public const uint   GL_VERSION=0x1f02;
		public const uint   GL_EXTENSIONS=0x1f03;

		/*TextureCoordName*/
		public const uint   GL_S=0x2000;
		public const uint   GL_T=0x2001;
		public const uint   GL_R=0x2002;
		public const uint   GL_Q=0x2003;

		/*TexCoordPoint erType*/
		/*GL_SHORT*/
		/*GL_INT */
		/*GL_FLOAT */
		/*GL_DOUBLE*/

		/*TextureEnvMode*/
		public const uint   GL_MODULATE=0x2100;
		public const uint   GL_DECAL=0x2101;
		/*GL_BLEND*/
		/*GL_REPLACE*/

		/*TextureEnvParameter*/
		public const uint   GL_TEXTURE_ENV_MODE=0x2200;
		public const uint   GL_TEXTURE_ENV_COLOR=0x2201;

		/*TextureEnvTarget*/
		public const uint   GL_TEXTURE_ENV=0x2300;

		/*TextureGenMode*/
		public const uint   GL_EYE_LINEAR=0x2400;
		public const uint   GL_OBJECT_LINEAR=0x2401;
		public const uint   GL_SPHERE_MAP=0x2402;

		/*TextureGenParameter*/
		public const uint   GL_TEXTURE_GEN_MODE=0x2500;
		public const uint   GL_OBJECT_PLANE=0x2501;
		public const uint   GL_EYE_PLANE=0x2502;

		/*TextureMagFilter*/
		public const uint   GL_NEAREST=0x2600;
		public const uint   GL_LINEAR=0x2601;

		/*TextureMinFilter*/
		/*GL_NEAREST*/
		/*GL_LINEAR*/
		public const uint   GL_NEAREST_MIPMAP_NEAREST=0x2700;
		public const uint   GL_LINEAR_MIPMAP_NEAREST=0x2701;
		public const uint   GL_NEAREST_MIPMAP_LINEAR=0x2702;
		public const uint   GL_LINEAR_MIPMAP_LINEAR=0x2703;

		/*TextureParameterName*/
		public const uint   GL_TEXTURE_MAG_FILTER=0x2800;
		public const uint   GL_TEXTURE_MIN_FILTER=0x2801;
		public const uint   GL_TEXTURE_WRAP_S=0x2802;
		public const uint   GL_TEXTURE_WRAP_T=0x2803;
		/*GL_TEXTURE_BORDER_COLOR*/
		/*GL_TEXTURE_PRIORITY*/

		/*TextureTarget*/
		/*GL_TEXTURE_1D*/
		/*GL_TEXTURE_2D*/
		/*GL_PROXY_TEXTURE_1D*/
		/*GL_PROXY_TEXTURE_2D*/

		/*TextureWrapMode*/
		public const uint   GL_CLAMP=0x2900;
		public const uint   GL_REPEAT=0x2901;

		/*VertexPoint erType*/
		/*GL_SHORT*/
		/*GL_INT */
		/*GL_FLOAT */
		/*GL_DOUBLE*/

		/*ClientAttribMask*/
		public const uint   GL_CLIENT_PIXEL_STORE_BIT=0x00000001;
		public const uint   GL_CLIENT_VERTEX_ARRAY_BIT=0x00000002;
		public const uint   GL_CLIENT_ALL_ATTRIB_BITS=0xffffffff;

		/*polygon_offset*/
		public const uint   GL_POLYGON_OFFSET_FACTOR=0x8038;
		public const uint   GL_POLYGON_OFFSET_UNITS=0x2a00;
		public const uint   GL_POLYGON_OFFSET_POint =0x2a01;
		public const uint   GL_POLYGON_OFFSET_LINE=0x2a02;
		public const uint   GL_POLYGON_OFFSET_FILL=0x8037;

		/*texture*/
		public const uint   GL_ALPHA4=0x803b;
		public const uint   GL_ALPHA8=0x803c;
		public const uint   GL_ALPHA12=0x803d;
		public const uint   GL_ALPHA16=0x803e;
		public const uint   GL_LUMINANCE4=0x803f;
		public const uint   GL_LUMINANCE8=0x8040;
		public const uint   GL_LUMINANCE12=0x8041;
		public const uint   GL_LUMINANCE16=0x8042;
		public const uint   GL_LUMINANCE4_ALPHA4=0x8043;
		public const uint   GL_LUMINANCE6_ALPHA2=0x8044;
		public const uint   GL_LUMINANCE8_ALPHA8=0x8045;
		public const uint   GL_LUMINANCE12_ALPHA4=0x8046;
		public const uint   GL_LUMINANCE12_ALPHA12=0x8047;
		public const uint   GL_LUMINANCE16_ALPHA16=0x8048;
		public const uint   GL_INTENSITY=0x8049;
		public const uint   GL_INTENSITY4=0x804a;
		public const uint   GL_INTENSITY8=0x804b;
		public const uint   GL_INTENSITY12=0x804c;
		public const uint   GL_INTENSITY16=0x804d;
		public const uint   GL_R3_G3_B2=0x2a10;
		public const uint   GL_RGB4=0x804f;
		public const uint   GL_RGB5=0x8050;
		public const uint   GL_RGB8=0x8051;
		public const uint   GL_RGB10=0x8052;
		public const uint   GL_RGB12=0x8053;
		public const uint   GL_RGB16=0x8054;
		public const uint   GL_RGBA2=0x8055;
		public const uint   GL_RGBA4=0x8056;
		public const uint   GL_RGB5_A1=0x8057;
		public const uint   GL_RGBA8=0x8058;
		public const uint   GL_RGB10_A2=0x8059;
		public const uint   GL_RGBA12=0x805a;
		public const uint   GL_RGBA16=0x805b;
		public const uint   GL_TEXTURE_RED_SIZE=0x805c;
		public const uint   GL_TEXTURE_GREEN_SIZE=0x805d;
		public const uint   GL_TEXTURE_BLUE_SIZE=0x805e;
		public const uint   GL_TEXTURE_ALPHA_SIZE=0x805f;
		public const uint   GL_TEXTURE_LUMINANCE_SIZE=0x8060;
		public const uint   GL_TEXTURE_INTENSITY_SIZE=0x8061;
		public const uint   GL_PROXY_TEXTURE_1D=0x8063;
		public const uint   GL_PROXY_TEXTURE_2D=0x8064;

		/*texture_object*/
		public const uint   GL_TEXTURE_PRIORITY=0x8066;
		public const uint   GL_TEXTURE_RESIDENT=0x8067;
		public const uint   GL_TEXTURE_BINDING_1D=0x8068;
		public const uint   GL_TEXTURE_BINDING_2D=0x8069;

		/*vertex_array*/
		public const uint   GL_VERTEX_ARRAY=0x8074;
		public const uint   GL_NORMAL_ARRAY=0x8075;
		public const uint   GL_COLOR_ARRAY=0x8076;
		public const uint   GL_INDEX_ARRAY=0x8077;
		public const uint   GL_TEXTURE_COORD_ARRAY=0x8078;
		public const uint   GL_EDGE_FLAG_ARRAY=0x8079;
		public const uint   GL_VERTEX_ARRAY_SIZE=0x807a;
		public const uint   GL_VERTEX_ARRAY_TYPE=0x807b;
		public const uint   GL_VERTEX_ARRAY_STRIDE=0x807c;
		public const uint   GL_NORMAL_ARRAY_TYPE=0x807e;
		public const uint   GL_NORMAL_ARRAY_STRIDE=0x807f;
		public const uint   GL_COLOR_ARRAY_SIZE=0x8081;
		public const uint   GL_COLOR_ARRAY_TYPE=0x8082;
		public const uint   GL_COLOR_ARRAY_STRIDE=0x8083;
		public const uint   GL_INDEX_ARRAY_TYPE=0x8085;
		public const uint   GL_INDEX_ARRAY_STRIDE=0x8086;
		public const uint   GL_TEXTURE_COORD_ARRAY_SIZE=0x8088;
		public const uint   GL_TEXTURE_COORD_ARRAY_TYPE=0x8089;
		public const uint   GL_TEXTURE_COORD_ARRAY_STRIDE=0x808a;
		public const uint   GL_EDGE_FLAG_ARRAY_STRIDE=0x808c;
		public const uint   GL_VERTEX_ARRAY_POINTER=0x808e;
		public const uint   GL_NORMAL_ARRAY_POINTER=0x808f;
		public const uint   GL_COLOR_ARRAY_POINTER=0x8090;
		public const uint   GL_INDEX_ARRAY_POINTER=0x8091;
		public const uint   GL_TEXTURE_COORD_ARRAY_POINTER=0x8092;
		public const uint   GL_EDGE_FLAG_ARRAY_POINTER=0x8093;
		public const uint   GL_V2F=0x2a20;
		public const uint   GL_V3F=0x2a21;
		public const uint   GL_C4UB_V2F=0x2a22;
		public const uint   GL_C4UB_V3F=0x2a23;
		public const uint   GL_C3F_V3F=0x2a24;
		public const uint   GL_N3F_V3F=0x2a25;
		public const uint   GL_C4F_N3F_V3F=0x2a26;
		public const uint   GL_T2F_V3F=0x2a27;
		public const uint   GL_T4F_V4F=0x2a28;
		public const uint   GL_T2F_C4UB_V3F=0x2a29;
		public const uint   GL_T2F_C3F_V3F=0x2a2a;
		public const uint   GL_T2F_N3F_V3F=0x2a2b;
		public const uint   GL_T2F_C4F_N3F_V3F=0x2a2c;
		public const uint   GL_T4F_C4F_N3F_V4F=0x2a2d;

		/*Extensions*/
		public const uint   GL_EXT_vertex_array=1;
		public const uint   GL_EXT_bgra=1;
		public const uint   GL_EXT_paletted_texture=1;
		public const uint   GL_WIN_swap_hint =1;
		public const uint   GL_WIN_draw_range_elements=1;
		//#defineGL_WIN_phong_shading1
		//#defineGL_WIN_specular_fog1

		/*EXT_vertex_array*/
		public const uint   GL_VERTEX_ARRAY_EXT=0x8074;
		public const uint   GL_NORMAL_ARRAY_EXT=0x8075;
		public const uint   GL_COLOR_ARRAY_EXT=0x8076;
		public const uint   GL_INDEX_ARRAY_EXT=0x8077;
		public const uint   GL_TEXTURE_COORD_ARRAY_EXT=0x8078;
		public const uint   GL_EDGE_FLAG_ARRAY_EXT=0x8079;
		public const uint   GL_VERTEX_ARRAY_SIZE_EXT=0x807a;
		public const uint   GL_VERTEX_ARRAY_TYPE_EXT=0x807b;
		public const uint   GL_VERTEX_ARRAY_STRIDE_EXT=0x807c;
		public const uint   GL_VERTEX_ARRAY_COUNT_EXT=0x807d;
		public const uint   GL_NORMAL_ARRAY_TYPE_EXT=0x807e;
		public const uint   GL_NORMAL_ARRAY_STRIDE_EXT=0x807f;
		public const uint   GL_NORMAL_ARRAY_COUNT_EXT=0x8080;
		public const uint   GL_COLOR_ARRAY_SIZE_EXT=0x8081;
		public const uint   GL_COLOR_ARRAY_TYPE_EXT=0x8082;
		public const uint   GL_COLOR_ARRAY_STRIDE_EXT=0x8083;
		public const uint   GL_COLOR_ARRAY_COUNT_EXT=0x8084;
		public const uint   GL_INDEX_ARRAY_TYPE_EXT=0x8085;
		public const uint   GL_INDEX_ARRAY_STRIDE_EXT=0x8086;
		public const uint   GL_INDEX_ARRAY_COUNT_EXT=0x8087;
		public const uint   GL_TEXTURE_COORD_ARRAY_SIZE_EXT=0x8088;
		public const uint   GL_TEXTURE_COORD_ARRAY_TYPE_EXT=0x8089;
		public const uint   GL_TEXTURE_COORD_ARRAY_STRIDE_EXT=0x808a;
		public const uint   GL_TEXTURE_COORD_ARRAY_COUNT_EXT=0x808b;
		public const uint   GL_EDGE_FLAG_ARRAY_STRIDE_EXT=0x808c;
		public const uint   GL_EDGE_FLAG_ARRAY_COUNT_EXT=0x808d;
		public const uint   GL_VERTEX_ARRAY_POINTER_EXT=0x808e;
		public const uint   GL_NORMAL_ARRAY_POINTER_EXT=0x808f;
		public const uint   GL_COLOR_ARRAY_POINTER_EXT=0x8090;
		public const uint   GL_INDEX_ARRAY_POINTER_EXT=0x8091;
		public const uint   GL_TEXTURE_COORD_ARRAY_POINTER_EXT=0x8092;
		public const uint   GL_EDGE_FLAG_ARRAY_POINTER_EXT=0x8093;
		public const uint   GL_DOUBLE_EXT=GL_DOUBLE;

		/*EXT_bgra*/
		public const uint   GL_BGR_EXT=0x80e0;
		public const uint   GL_BGRA_EXT=0x80e1;

		/*EXT_paletted_texture*/

		/*These must match the GL_COLOR_TABLE_*_SGI enumerants*/
		public const uint   GL_COLOR_TABLE_FORMAT_EXT=0x80d8;
		public const uint   GL_COLOR_TABLE_WIDTH_EXT=0x80d9;
		public const uint   GL_COLOR_TABLE_RED_SIZE_EXT=0x80da;
		public const uint   GL_COLOR_TABLE_GREEN_SIZE_EXT=0x80db;
		public const uint   GL_COLOR_TABLE_BLUE_SIZE_EXT=0x80dc;
		public const uint   GL_COLOR_TABLE_ALPHA_SIZE_EXT=0x80dd;
		public const uint   GL_COLOR_TABLE_LUMINANCE_SIZE_EXT=0x80de;
		public const uint   GL_COLOR_TABLE_INTENSITY_SIZE_EXT=0x80df;

		public const uint   GL_COLOR_INDEX1_EXT=0x80e2;
		public const uint   GL_COLOR_INDEX2_EXT=0x80e3;
		public const uint   GL_COLOR_INDEX4_EXT=0x80e4;
		public const uint   GL_COLOR_INDEX8_EXT=0x80e5;
		public const uint   GL_COLOR_INDEX12_EXT=0x80e6;
		public const uint   GL_COLOR_INDEX16_EXT=0x80e7;

		/*WIN_draw_range_elements*/
		public const uint   GL_MAX_ELEMENTS_VERTICES_WIN=0x80e8;
		public const uint   GL_MAX_ELEMENTS_INDICES_WIN=0x80e9;

		/*WIN_phong_shading*/
		public const uint   GL_PHONG_WIN=0x80ea;
		public const uint   GL_PHONG_HINT_WIN=0x80eb;

		/*WIN_specular_fog*/
		public const uint   GL_FOG_SPECULAR_TEXTURE_WIN=0x80ec;

		/*For compatibility withOpenGL v1.0*/
		public const uint   GL_LOGIC_OP=GL_INDEX_LOGIC_OP;
		public const uint   GL_TEXTURE_COMPONENTS=GL_TEXTURE_INTERNAL_FORMAT;

		/*************************************************************/

		/*EXT_vertex_array*/

		/*WIN_draw_range_elements*/

		/*WIN_swap_hint */

		/*EXT_paletted_texture*/

		/*__GL_H__*/
		/*__gl_h_*/

		[DllImport(GL_DLL,EntryPoint ="glAccum")]
		public static extern void glAccum(uint  op,float valuex);
		[DllImport(GL_DLL,EntryPoint ="glAlphaFunc")]
		public static extern void glAlphaFunc(uint  func,float refx);
		[DllImport(GL_DLL,EntryPoint ="glAreTexturesResident")]
		public static extern uint glAreTexturesResident(int n,uint[] textures,uint[] residences);
		[DllImport(GL_DLL,EntryPoint ="glArrayElement")]
		public static extern void glArrayElement(int i);
		[DllImport(GL_DLL,EntryPoint ="glBegin")]
		public static extern void glBegin(uint  mode);
		[DllImport(GL_DLL,EntryPoint ="glBindTexture")]
		public static extern void glBindTexture(uint  target,uint  texture);
		[DllImport(GL_DLL,EntryPoint ="glBitmap")]
		public static extern void glBitmap(int width,int height,float xorig,float yorig,float xmove,float ymove,byte[] bitmap);
		[DllImport(GL_DLL,EntryPoint ="glBlendFunc")]
		public static extern void glBlendFunc(uint  sfactor,uint  dfactor);
		[DllImport(GL_DLL,EntryPoint ="glCallList")]
		public static extern void glCallList(uint  list);
		[DllImport(GL_DLL,EntryPoint ="glCallLists")]
        public static extern void glCallLists(int n, uint type, byte[] lists);
        [DllImport(GL_DLL, EntryPoint = "glCallLists")]
        public static extern void glCallLists(int n, uint type, char[] lists);
        [DllImport(GL_DLL, EntryPoint = "glCallLists")]
        public static extern void glCallLists(int n, uint type, object[] lists);
		[DllImport(GL_DLL,EntryPoint ="glClear")]
		public static extern void glClear(uint  mask);
		[DllImport(GL_DLL,EntryPoint ="glClearAccum")]
		public static extern void glClearAccum(float red,float green,float blue,float alpha);
		[DllImport(GL_DLL,EntryPoint ="glClearColor")]
		public static extern void glClearColor(float red,float green,float blue,float alpha);
		[DllImport(GL_DLL,EntryPoint ="glClearDepth")]
		public static extern void glClearDepth(double depth);
		[DllImport(GL_DLL,EntryPoint ="glClearIndex")]
		public static extern void glClearIndex(float c);
		[DllImport(GL_DLL,EntryPoint ="glClearStencil")]
		public static extern void glClearStencil(int s);
		[DllImport(GL_DLL,EntryPoint ="glClipPlane")]
		public static extern void glClipPlane(uint  plane,double[] equation);
		[DllImport(GL_DLL,EntryPoint ="glColor3b")]
		public static extern void glColor3b(sbyte red,sbyte green,sbyte blue);
		[DllImport(GL_DLL,EntryPoint ="glColor3bv")]
		public static extern void glColor3bv(sbyte[] v);
		[DllImport(GL_DLL,EntryPoint ="glColor3d")]
		public static extern void glColor3d(double red,double green,double blue);
		[DllImport(GL_DLL,EntryPoint ="glColor3dv")]
		public static extern void glColor3dv(double[] v);
		[DllImport(GL_DLL,EntryPoint ="glColor3f")]
		public static extern void glColor3f(float red,float green,float blue);
		[DllImport(GL_DLL,EntryPoint ="glColor3fv")]
		public static extern void glColor3fv(float[] v);
		[DllImport(GL_DLL,EntryPoint ="glColor3i")]
		public static extern void glColor3i(int red,int green,int blue);
		[DllImport(GL_DLL,EntryPoint ="glColor3iv")]
		public static extern void glColor3iv(int[] v);
		[DllImport(GL_DLL,EntryPoint ="glColor3s")]
		public static extern void glColor3s(short red,short green,short blue);
		[DllImport(GL_DLL,EntryPoint ="glColor3sv")]
		public static extern void glColor3sv(short[] v);
		[DllImport(GL_DLL,EntryPoint ="glColor3ub")]
		public static extern void glColor3ub(byte red,byte green,byte blue);
		[DllImport(GL_DLL,EntryPoint ="glColor3ubv")]
		public static extern void glColor3ubv(byte[] v);
		[DllImport(GL_DLL,EntryPoint ="glColor3ui")]
		public static extern void glColor3ui(uint  red,uint  green,uint  blue);
		[DllImport(GL_DLL,EntryPoint ="glColor3uiv")]
		public static extern void glColor3uiv(uint[] v);
		[DllImport(GL_DLL,EntryPoint ="glColor3us")]
		public static extern void glColor3us(ushort red,ushort green,ushort blue);
		[DllImport(GL_DLL,EntryPoint ="glColor3usv")]
		public static extern void glColor3usv(ushort[] v);
		[DllImport(GL_DLL,EntryPoint ="glColor4b")]
		public static extern void glColor4b(sbyte red,sbyte green,sbyte blue,sbyte alpha);
		[DllImport(GL_DLL,EntryPoint ="glColor4bv")]
		public static extern void glColor4bv(sbyte[] v);
		[DllImport(GL_DLL,EntryPoint ="glColor4d")]
		public static extern void glColor4d(double red,double green,double blue,double alpha);
		[DllImport(GL_DLL,EntryPoint ="glColor4dv")]
		public static extern void glColor4dv(double[] v);
		[DllImport(GL_DLL,EntryPoint ="glColor4f")]
		public static extern void glColor4f(float red,float green,float blue,float alpha);
		[DllImport(GL_DLL,EntryPoint ="glColor4fv")]
		public static extern void glColor4fv(float[] v);
		[DllImport(GL_DLL,EntryPoint ="glColor4i")]
		public static extern void glColor4i(int red,int green,int blue,int alpha);
		[DllImport(GL_DLL,EntryPoint ="glColor4iv")]
		public static extern void glColor4iv(int[] v);
		[DllImport(GL_DLL,EntryPoint ="glColor4s")]
		public static extern void glColor4s(short red,short green,short blue,short alpha);
		[DllImport(GL_DLL,EntryPoint ="glColor4sv")]
		public static extern void glColor4sv(short[] v);
		[DllImport(GL_DLL,EntryPoint ="glColor4ub")]
		public static extern void glColor4ub(byte red,byte green,byte blue,byte alpha);
		[DllImport(GL_DLL,EntryPoint ="glColor4ubv")]
		public static extern void glColor4ubv(byte[] v);
		[DllImport(GL_DLL,EntryPoint ="glColor4ui")]
		public static extern void glColor4ui(uint  red,uint  green,uint  blue,uint  alpha);
		[DllImport(GL_DLL,EntryPoint ="glColor4uiv")]
		public static extern void glColor4uiv(uint[] v);
		[DllImport(GL_DLL,EntryPoint ="glColor4us")]
		public static extern void glColor4us(ushort red,ushort green,ushort blue,ushort alpha);
		[DllImport(GL_DLL,EntryPoint ="glColor4usv")]
		public static extern void glColor4usv(ushort[] v);
		[DllImport(GL_DLL,EntryPoint ="glColorMask")]
		public static extern void glColorMask(byte red,byte green,byte blue,byte alpha);
		[DllImport(GL_DLL,EntryPoint ="glColorMaterial")]
		public static extern void glColorMaterial(uint  face,uint  mode);
		[DllImport(GL_DLL,EntryPoint ="glColorPointer")]
		public static extern void glColorPointer(int size,uint  type,int stride,object[] pointer);
		[DllImport(GL_DLL,EntryPoint ="glCopyPixels")]
		public static extern void glCopyPixels(int x,int y,int width,int height,uint  type);
		[DllImport(GL_DLL,EntryPoint ="glCopyTexImage1D")]
		public static extern void glCopyTexImage1D(uint  target,int level,uint  internalFormat,int x,int y,int width,int border);
		[DllImport(GL_DLL,EntryPoint ="glCopyTexImage2D")]
		public static extern void glCopyTexImage2D(uint  target,int level,uint  internalFormat,int x,int y,int width,int height,int border);
		[DllImport(GL_DLL,EntryPoint ="glCopyTexSubImage1D")]
		public static extern void glCopyTexSubImage1D(uint  target,int level,int xoffset,int x,int y,int width);
		[DllImport(GL_DLL,EntryPoint ="glCopyTexSubImage2D")]
		public static extern void glCopyTexSubImage2D(uint  target,int level,int xoffset,int yoffset,int x,int y,int width,int height);
		[DllImport(GL_DLL,EntryPoint ="glCullFace")]
		public static extern void glCullFace(uint  mode);
		[DllImport(GL_DLL,EntryPoint ="glDeleteLists")]
		public static extern void glDeleteLists(uint  list,int range);
		[DllImport(GL_DLL,EntryPoint ="glDeleteTextures")]
		public static extern void glDeleteTextures(int n,uint[] textures);
		[DllImport(GL_DLL,EntryPoint ="glDepthFunc")]
		public static extern void glDepthFunc(uint  func);
		[DllImport(GL_DLL,EntryPoint ="glDepthMask")]
		public static extern void glDepthMask(byte flag);
		[DllImport(GL_DLL,EntryPoint ="glDepthRange")]
		public static extern void glDepthRange(double zNear,double zFar);
		[DllImport(GL_DLL,EntryPoint ="glDisable")]
		public static extern void glDisable(uint  cap);
		[DllImport(GL_DLL,EntryPoint ="glDisableClientState")]
		public static extern void glDisableClientState(uint  array);
		[DllImport(GL_DLL,EntryPoint ="glDrawArrays")]
		public static extern void glDrawArrays(uint  mode,int first,int count);
		[DllImport(GL_DLL,EntryPoint ="glDrawBuffer")]
		public static extern void glDrawBuffer(uint  mode);
		[DllImport(GL_DLL,EntryPoint ="glDrawElements")]
		public static extern void glDrawElements(uint  mode,int count,uint  type,object[] indices);
		[DllImport(GL_DLL,EntryPoint ="glDrawPixels")]
		//public static extern void glDrawPixels(int width,int height,uint  format,uint  type,object[] pixels);
		public static extern void glDrawPixels(int width,int height,uint  format,uint  type, byte[] pixels);
		[DllImport(GL_DLL,EntryPoint ="glEdgeFlag")]
		public static extern void glEdgeFlag(byte flag);
		[DllImport(GL_DLL,EntryPoint ="glEdgeFlagPointer")]
		public static extern void glEdgeFlagPointer(int stride,object[] pointer);
		[DllImport(GL_DLL,EntryPoint ="glEdgeFlagv")]
		public static extern void glEdgeFlagv(uint[] flag);
		[DllImport(GL_DLL,EntryPoint ="glEnable")]
		public static extern void glEnable(uint  cap);
		[DllImport(GL_DLL,EntryPoint ="glEnableClientState")]
		public static extern void glEnableClientState(uint  array);
		[DllImport(GL_DLL,EntryPoint ="glEnd")]
		public static extern void glEnd();
		[DllImport(GL_DLL,EntryPoint ="glEndList")]
		public static extern void glEndList();
		[DllImport(GL_DLL,EntryPoint ="glEvalCoord1d")]
		public static extern void glEvalCoord1d(double u);
		[DllImport(GL_DLL,EntryPoint ="glEvalCoord1dv")]
		public static extern void glEvalCoord1dv(double[] u);
		[DllImport(GL_DLL,EntryPoint ="glEvalCoord1f")]
		public static extern void glEvalCoord1f(float u);
		[DllImport(GL_DLL,EntryPoint ="glEvalCoord1fv")]
		public static extern void glEvalCoord1fv(float[] u);
		[DllImport(GL_DLL,EntryPoint ="glEvalCoord2d")]
		public static extern void glEvalCoord2d(double u,double v);
		[DllImport(GL_DLL,EntryPoint ="glEvalCoord2dv")]
		public static extern void glEvalCoord2dv(double[] u);
		[DllImport(GL_DLL,EntryPoint ="glEvalCoord2f")]
		public static extern void glEvalCoord2f(float u,float v);
		[DllImport(GL_DLL,EntryPoint ="glEvalCoord2fv")]
		public static extern void glEvalCoord2fv(float[] u);
		[DllImport(GL_DLL,EntryPoint ="glEvalMesh1")]
		public static extern void glEvalMesh1(uint  mode,int i1,int i2);
		[DllImport(GL_DLL,EntryPoint ="glEvalMesh2")]
		public static extern void glEvalMesh2(uint  mode,int i1,int i2,int j1,int j2);
		[DllImport(GL_DLL,EntryPoint ="glEvalPoint 1")]
		public static extern void glEvalPoint1(int i);
		[DllImport(GL_DLL,EntryPoint ="glEvalPoint 2")]
		public static extern void glEvalPoint2(int i,int j);
		[DllImport(GL_DLL,EntryPoint ="glFeedbackBuffer")]
		public static extern void glFeedbackBuffer(int size,uint  type,float[] buffer);
		[DllImport(GL_DLL,EntryPoint ="glFinish")]
		public static extern void glFinish();
		[DllImport(GL_DLL,EntryPoint ="glFlush")]
		public static extern void glFlush();
		[DllImport(GL_DLL,EntryPoint ="glFogf")]
		public static extern void glFogf(uint  pname,float param);
		[DllImport(GL_DLL,EntryPoint ="glFogfv")]
		public static extern void glFogfv(uint  pname,float[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glFogi")]
		public static extern void glFogi(uint  pname,int param);
		[DllImport(GL_DLL,EntryPoint ="glFogiv")]
		public static extern void glFogiv(uint  pname,int[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glFrontFace")]
		public static extern void glFrontFace(uint  mode);
		[DllImport(GL_DLL,EntryPoint ="glFrustum")]
		public static extern void glFrustum(double left,double right,double bottom,double top,double zNear,double zFar);
		[DllImport(GL_DLL,EntryPoint ="glGenLists")]
		public static extern uint glGenLists(int range);
		[DllImport(GL_DLL,EntryPoint ="glGenTextures")]
		public static extern void glGenTextures(int n,uint[] textures);
		[DllImport(GL_DLL,EntryPoint ="glGetBooleanv")]
		public static extern void glGetBooleanv(uint  pname,uint[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glGetClipPlane")]
		public static extern void glGetClipPlane(uint  plane,double[] equation);
		[DllImport(GL_DLL,EntryPoint ="glGetDoublev")]
		public static extern void glGetDoublev(uint  pname,double[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glGetError")]
		public static extern uint  glGetError();
		[DllImport(GL_DLL,EntryPoint ="glGetFloatv")]
		public static extern void glGetFloatv(uint  pname,float[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glGetIntegerv")]
		public static extern void glGetIntegerv(uint  pname,int[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glGetLightfv")]
		public static extern void glGetLightfv(uint  light,uint  pname,float[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glGetLightiv")]
		public static extern void glGetLightiv(uint  light,uint  pname,int[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glGetMapdv")]
		public static extern void glGetMapdv(uint  target,uint  query,double[] v);
		[DllImport(GL_DLL,EntryPoint ="glGetMapfv")]
		public static extern void glGetMapfv(uint  target,uint  query,float[] v);
		[DllImport(GL_DLL,EntryPoint ="glGetMapiv")]
		public static extern void glGetMapiv(uint  target,uint  query,int[] v);
		[DllImport(GL_DLL,EntryPoint ="glGetMaterialfv")]
		public static extern void glGetMaterialfv(uint  face,uint  pname,float[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glGetMaterialiv")]
		public static extern void glGetMaterialiv(uint  face,uint  pname,int[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glGetPixelMapfv")]
		public static extern void glGetPixelMapfv(uint  map,float[] values);
		[DllImport(GL_DLL,EntryPoint ="glGetPixelMapuiv")]
		public static extern void glGetPixelMapuiv(uint  map,uint[] values);
		[DllImport(GL_DLL,EntryPoint ="glGetPixelMapusv")]
		public static extern void glGetPixelMapusv(uint  map,ushort[] values);
		[DllImport(GL_DLL,EntryPoint ="glGetPointerv")]
		public static extern void glGetPointerv(uint  pname,object[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glGetPolygonStipple")]
		public static extern void glGetPolygonStipple(byte[] mask);
		[DllImport(GL_DLL,EntryPoint ="glGetString")]
		//public static extern byte[] glGetString(uint  name);
		public static extern IntPtr glGetString(uint  name);
		[DllImport(GL_DLL,EntryPoint ="glGetTexEnvfv")]
		public static extern void glGetTexEnvfv(uint  target,uint  pname,float[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glGetTexEnviv")]
		public static extern void glGetTexEnviv(uint  target,uint  pname,int[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glGetTexGendv")]
		public static extern void glGetTexGendv(uint  coord,uint  pname,double[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glGetTexGenfv")]
		public static extern void glGetTexGenfv(uint  coord,uint  pname,float[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glGetTexGeniv")]
		public static extern void glGetTexGeniv(uint  coord,uint  pname,int[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glGetTexImage")]
		public static extern void glGetTexImage(uint  target,int level,uint  format,uint  type,object[] pixels);
		[DllImport(GL_DLL,EntryPoint ="glGetTexLevelParameterfv")]
		public static extern void glGetTexLevelParameterfv(uint  target,int level,uint  pname,float[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glGetTexLevelParameteriv")]
		public static extern void glGetTexLevelParameteriv(uint  target,int level,uint  pname,int[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glGetTexParameterfv")]
		public static extern void glGetTexParameterfv(uint  target,uint  pname,float[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glGetTexParameteriv")]
		public static extern void glGetTexParameteriv(uint  target,uint  pname,int[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glHint")]
		public static extern void glHint (uint  target,uint  mode);
		[DllImport(GL_DLL,EntryPoint ="glIndexMask")]
		public static extern void glIndexMask(uint  mask);
		[DllImport(GL_DLL,EntryPoint ="glIndexPoint er")]
		public static extern void glIndexPointer(uint  type,int stride,object[] pointer);
		[DllImport(GL_DLL,EntryPoint ="glIndexd")]
		public static extern void glIndexd(double c);
		[DllImport(GL_DLL,EntryPoint ="glIndexdv")]
		public static extern void glIndexdv(double[] c);
		[DllImport(GL_DLL,EntryPoint ="glIndexf")]
		public static extern void glIndexf(float c);
		[DllImport(GL_DLL,EntryPoint ="glIndexfv")]
		public static extern void glIndexfv(float[] c);
		[DllImport(GL_DLL,EntryPoint ="glIndexi")]
		public static extern void glIndexi(int c);
		[DllImport(GL_DLL,EntryPoint ="glIndexiv")]
		public static extern void glIndexiv(int[] c);
		[DllImport(GL_DLL,EntryPoint ="glIndexs")]
		public static extern void glIndexs(short c);
		[DllImport(GL_DLL,EntryPoint ="glIndexsv")]
		public static extern void glIndexsv(short[] c);
		[DllImport(GL_DLL,EntryPoint ="glIndexub")]
		public static extern void glIndexub(byte c);
		[DllImport(GL_DLL,EntryPoint ="glIndexubv")]
		public static extern void glIndexubv(byte[] c);
		[DllImport(GL_DLL,EntryPoint ="glInitNames")]
		public static extern void glInitNames();
		[DllImport(GL_DLL,EntryPoint ="glInterleavedArrays")]
		public static extern void glInterleavedArrays(uint  format,int stride,object[] pointer);
		[DllImport(GL_DLL,EntryPoint ="glIsEnabled")]
		public static extern uint glIsEnabled(uint cap);
		[DllImport(GL_DLL,EntryPoint ="glIsList")]
		public static extern uint glIsList(uint list);
		[DllImport(GL_DLL,EntryPoint ="glIsTexture")]
		public static extern uint glIsTexture(uint texture);
		[DllImport(GL_DLL,EntryPoint ="glLightModelf")]
		public static extern void glLightModelf(uint  pname,float param);
		[DllImport(GL_DLL,EntryPoint ="glLightModelfv")]
		public static extern void glLightModelfv(uint  pname,float[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glLightModeli")]
		public static extern void glLightModeli(uint  pname,int param);
		[DllImport(GL_DLL,EntryPoint ="glLightModeliv")]
		public static extern void glLightModeliv(uint  pname,int[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glLightf")]
		public static extern void glLightf(uint light,uint  pname,float param);
		[DllImport(GL_DLL,EntryPoint ="glLightfv")]
		public static extern void glLightfv(uint  light,uint  pname,float[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glLighti")]
		public static extern void glLighti(uint  light,uint  pname,int param);
		[DllImport(GL_DLL,EntryPoint ="glLightiv")]
		public static extern void glLightiv(uint  light,uint  pname,int[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glLineStipple")]
		public static extern void glLineStipple(int factor,ushort pattern);
		[DllImport(GL_DLL,EntryPoint ="glLineWidth")]
		public static extern void glLineWidth(float width);
		[DllImport(GL_DLL,EntryPoint ="glListBase")]
		public static extern void glListBase(uint  basex);
		[DllImport(GL_DLL,EntryPoint ="glLoadIdentity")]
		public static extern void glLoadIdentity();
		[DllImport(GL_DLL,EntryPoint ="glLoadMatrixd")]
		public static extern void glLoadMatrixd(double[] m);
		[DllImport(GL_DLL,EntryPoint ="glLoadMatrixf")]
		public static extern void glLoadMatrixf(float[] m);
		[DllImport(GL_DLL,EntryPoint ="glLoadName")]
		public static extern void glLoadName(uint  name);
		[DllImport(GL_DLL,EntryPoint ="glLogicOp")]
		public static extern void glLogicOp(uint  opcode);
		[DllImport(GL_DLL,EntryPoint ="glMap1d")]
		public static extern void glMap1d(uint  target,double u1,double u2,int stride,int order,double[] points);
		[DllImport(GL_DLL,EntryPoint ="glMap1f")]
		public static extern void glMap1f(uint  target,float u1,float u2,int stride,int order,float[] points);
		[DllImport(GL_DLL,EntryPoint ="glMap2d")]
		public static extern void glMap2d(uint  target,double u1,double u2,int ustride,int uorder,double v1,double v2,int vstride,int vorder,double[] points);
		[DllImport(GL_DLL,EntryPoint ="glMap2f")]
		public static extern void glMap2f(uint  target,float u1,float u2,int ustride,int uorder,float v1,float v2,int vstride,int vorder,float[] points);
		[DllImport(GL_DLL,EntryPoint ="glMapGrid1d")]
		public static extern void glMapGrid1d(int un,double u1,double u2);
		[DllImport(GL_DLL,EntryPoint ="glMapGrid1f")]
		public static extern void glMapGrid1f(int un,float u1,float u2);
		[DllImport(GL_DLL,EntryPoint ="glMapGrid2d")]
		public static extern void glMapGrid2d(int un,double u1,double u2,int vn,double v1,double v2);
		[DllImport(GL_DLL,EntryPoint ="glMapGrid2f")]
		public static extern void glMapGrid2f(int un,float u1,float u2,int vn,float v1,float v2);
		[DllImport(GL_DLL,EntryPoint ="glMaterialf")]
		public static extern void glMaterialf(uint  face,uint  pname,float param);
		[DllImport(GL_DLL,EntryPoint ="glMaterialfv")]
		public static extern void glMaterialfv(uint  face,uint  pname,float[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glMateriali")]
		public static extern void glMateriali(uint  face,uint  pname,int param);
		[DllImport(GL_DLL,EntryPoint ="glMaterialiv")]
		public static extern void glMaterialiv(uint  face,uint  pname,int[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glMatrixMode")]
		public static extern void glMatrixMode(uint  mode);
		[DllImport(GL_DLL,EntryPoint ="glMultMatrixd")]
		public static extern void glMultMatrixd(double[] m);
		[DllImport(GL_DLL,EntryPoint ="glMultMatrixf")]
		public static extern void glMultMatrixf(float[] m);
		[DllImport(GL_DLL,EntryPoint ="glNewList")]
		public static extern void glNewList(uint  list,uint  mode);
		[DllImport(GL_DLL,EntryPoint ="glNormal3b")]
		public static extern void glNormal3b(sbyte nx,sbyte ny,sbyte nz);
		[DllImport(GL_DLL,EntryPoint ="glNormal3bv")]
		public static extern void glNormal3bv(sbyte[] v);
		[DllImport(GL_DLL,EntryPoint ="glNormal3d")]
		public static extern void glNormal3d(double nx,double ny,double nz);
		[DllImport(GL_DLL,EntryPoint ="glNormal3dv")]
		public static extern void glNormal3dv(double[] v);
		[DllImport(GL_DLL,EntryPoint ="glNormal3f")]
		public static extern void glNormal3f(float nx,float ny,float nz);
		[DllImport(GL_DLL,EntryPoint ="glNormal3fv")]
		public static extern void glNormal3fv(float[] v);
		[DllImport(GL_DLL,EntryPoint ="glNormal3i")]
		public static extern void glNormal3i(int nx,int ny,int nz);
		[DllImport(GL_DLL,EntryPoint ="glNormal3iv")]
		public static extern void glNormal3iv(int[] v);
		[DllImport(GL_DLL,EntryPoint ="glNormal3s")]
		public static extern void glNormal3s(short nx,short ny,short nz);
		[DllImport(GL_DLL,EntryPoint ="glNormal3sv")]
		public static extern void glNormal3sv(short[] v);
		[DllImport(GL_DLL,EntryPoint ="glNormalPointer")]
		public static extern void glNormalPointer(uint  type,int stride,object[] pointer);
		[DllImport(GL_DLL,EntryPoint ="glOrtho")]
		public static extern void glOrtho(double left,double right,double bottom,double top,double zNear,double zFar);
		[DllImport(GL_DLL,EntryPoint ="glPassThrough")]
		public static extern void glPassThrough(float token);
		[DllImport(GL_DLL,EntryPoint ="glPixelMapfv")]
		public static extern void glPixelMapfv(uint  map,int mapsize,float[] values);
		[DllImport(GL_DLL,EntryPoint ="glPixelMapuiv")]
		public static extern void glPixelMapuiv(uint  map,int mapsize,uint[] values);
		[DllImport(GL_DLL,EntryPoint ="glPixelMapusv")]
		public static extern void glPixelMapusv(uint  map,int mapsize,ushort[] values);
		[DllImport(GL_DLL,EntryPoint ="glPixelStoref")]
		public static extern void glPixelStoref(uint  pname,float param);
		[DllImport(GL_DLL,EntryPoint ="glPixelStorei")]
		public static extern void glPixelStorei(uint  pname,int param);
		[DllImport(GL_DLL,EntryPoint ="glPixelTransferf")]
		public static extern void glPixelTransferf(uint  pname,float param);
		[DllImport(GL_DLL,EntryPoint ="glPixelTransferi")]
		public static extern void glPixelTransferi(uint  pname,int param);
		[DllImport(GL_DLL,EntryPoint ="glPixelZoom")]
		public static extern void glPixelZoom(float xfactor,float yfactor);
		[DllImport(GL_DLL,EntryPoint ="glPointSize")]
		public static extern void glPointSize(float size);
		[DllImport(GL_DLL,EntryPoint ="glPolygonMode")]
		public static extern void glPolygonMode(uint  face,uint  mode);
		[DllImport(GL_DLL,EntryPoint ="glPolygonOffset")]
		public static extern void glPolygonOffset(float factor,float units);
		[DllImport(GL_DLL,EntryPoint ="glPolygonStipple")]
		public static extern void glPolygonStipple(byte[] mask);
		[DllImport(GL_DLL,EntryPoint ="glPopAttrib")]
		public static extern void glPopAttrib();
		[DllImport(GL_DLL,EntryPoint ="glPopClientAttrib")]
		public static extern void glPopClientAttrib();
		[DllImport(GL_DLL,EntryPoint ="glPopMatrix")]
		public static extern void glPopMatrix();
		[DllImport(GL_DLL,EntryPoint ="glPopName")]
		public static extern void glPopName();
		[DllImport(GL_DLL,EntryPoint ="glPrioritizeTextures")]
		public static extern void glPrioritizeTextures(int n,uint[] textures,float[] priorities);
		[DllImport(GL_DLL,EntryPoint ="glPushAttrib")]
		public static extern void glPushAttrib(uint  mask);
		[DllImport(GL_DLL,EntryPoint ="glPushClientAttrib")]
		public static extern void glPushClientAttrib(uint  mask);
		[DllImport(GL_DLL,EntryPoint ="glPushMatrix")]
		public static extern void glPushMatrix();
		[DllImport(GL_DLL,EntryPoint ="glPushName")]
		public static extern void glPushName(uint  name);
		[DllImport(GL_DLL,EntryPoint ="glRasterPos2d")]
		public static extern void glRasterPos2d(double x,double y);
		[DllImport(GL_DLL,EntryPoint ="glRasterPos2dv")]
		public static extern void glRasterPos2dv(double[] v);
		[DllImport(GL_DLL,EntryPoint ="glRasterPos2f")]
		public static extern void glRasterPos2f(float x,float y);
		[DllImport(GL_DLL,EntryPoint ="glRasterPos2fv")]
		public static extern void glRasterPos2fv(float[] v);
		[DllImport(GL_DLL,EntryPoint ="glRasterPos2i")]
		public static extern void glRasterPos2i(int x,int y);
		[DllImport(GL_DLL,EntryPoint ="glRasterPos2iv")]
		public static extern void glRasterPos2iv(int[] v);
		[DllImport(GL_DLL,EntryPoint ="glRasterPos2s")]
		public static extern void glRasterPos2s(short x,short y);
		[DllImport(GL_DLL,EntryPoint ="glRasterPos2sv")]
		public static extern void glRasterPos2sv(short[] v);
		[DllImport(GL_DLL,EntryPoint ="glRasterPos3d")]
		public static extern void glRasterPos3d(double x,double y,double z);
		[DllImport(GL_DLL,EntryPoint ="glRasterPos3dv")]
		public static extern void glRasterPos3dv(double[] v);
		[DllImport(GL_DLL,EntryPoint ="glRasterPos3f")]
		public static extern void glRasterPos3f(float x,float y,float z);
		[DllImport(GL_DLL,EntryPoint ="glRasterPos3fv")]
		public static extern void glRasterPos3fv(float[] v);
		[DllImport(GL_DLL,EntryPoint ="glRasterPos3i")]
		public static extern void glRasterPos3i(int x,int y,int z);
		[DllImport(GL_DLL,EntryPoint ="glRasterPos3iv")]
		public static extern void glRasterPos3iv(int[] v);
		[DllImport(GL_DLL,EntryPoint ="glRasterPos3s")]
		public static extern void glRasterPos3s(short x,short y,short z);
		[DllImport(GL_DLL,EntryPoint ="glRasterPos3sv")]
		public static extern void glRasterPos3sv(short[] v);
		[DllImport(GL_DLL,EntryPoint ="glRasterPos4d")]
		public static extern void glRasterPos4d(double x,double y,double z,double w);
		[DllImport(GL_DLL,EntryPoint ="glRasterPos4dv")]
		public static extern void glRasterPos4dv(double[] v);
		[DllImport(GL_DLL,EntryPoint ="glRasterPos4f")]
		public static extern void glRasterPos4f(float x,float y,float z,float w);
		[DllImport(GL_DLL,EntryPoint ="glRasterPos4fv")]
		public static extern void glRasterPos4fv(float[] v);
		[DllImport(GL_DLL,EntryPoint ="glRasterPos4i")]
		public static extern void glRasterPos4i(int x,int y,int z,int w);
		[DllImport(GL_DLL,EntryPoint ="glRasterPos4iv")]
		public static extern void glRasterPos4iv(int[] v);
		[DllImport(GL_DLL,EntryPoint ="glRasterPos4s")]
		public static extern void glRasterPos4s(short x,short y,short z,short w);
		[DllImport(GL_DLL,EntryPoint ="glRasterPos4sv")]
		public static extern void glRasterPos4sv(short[] v);
		[DllImport(GL_DLL,EntryPoint ="glReadBuffer")]
		public static extern void glReadBuffer(uint  mode);
		[DllImport(GL_DLL,EntryPoint ="glReadPixels")]
		public static extern void glReadPixels(int x,int y,int width,int height,uint  format,uint  type,object[] pixels);
		[DllImport(GL_DLL,EntryPoint ="glRectd")]
		public static extern void glRectd(double x1,double y1,double x2,double y2);
		[DllImport(GL_DLL,EntryPoint ="glRectdv")]
		public static extern void glRectdv(double[] v1,double[]v2);
		[DllImport(GL_DLL,EntryPoint ="glRectf")]
		public static extern void glRectf(float x1,float y1,float x2,float y2);
		[DllImport(GL_DLL,EntryPoint ="glRectfv")]
		public static extern void glRectfv(float[] v1,float[] v2);
		[DllImport(GL_DLL,EntryPoint ="glRecti")]
		public static extern void glRecti(int x1,int y1,int x2,int y2);
		[DllImport(GL_DLL,EntryPoint ="glRectiv")]
		public static extern void glRectiv(int[] v1,int[] v2);
		[DllImport(GL_DLL,EntryPoint ="glRects")]
		public static extern void glRects(short x1,short y1,short x2,short y2);
		[DllImport(GL_DLL,EntryPoint ="glRectsv")]
		public static extern void glRectsv(short[] v1,short[] v2);
		[DllImport(GL_DLL,EntryPoint ="glRenderMode")]
		public static extern uint  glRenderMode(uint  mode);
		[DllImport(GL_DLL,EntryPoint ="glRotated")]
		public static extern void glRotated(double angle,double x,double y,double z);
		[DllImport(GL_DLL,EntryPoint ="glRotatef")]
		public static extern void glRotatef(float angle,float x,float y,float z);
		[DllImport(GL_DLL,EntryPoint ="glScaled")]
		public static extern void glScaled(double x,double y,double z);
		[DllImport(GL_DLL,EntryPoint ="glScalef")]
		public static extern void glScalef(float x,float y,float z);
		[DllImport(GL_DLL,EntryPoint ="glScissor")]
		public static extern void glScissor(int x,int y,int width,int height);
		[DllImport(GL_DLL,EntryPoint ="glSelectBuffer")]
		public static extern void glSelectBuffer(int size,uint[] buffer);
		[DllImport(GL_DLL,EntryPoint ="glShadeModel")]
		public static extern void glShadeModel(uint  mode);
		[DllImport(GL_DLL,EntryPoint ="glStencilFunc")]
		public static extern void glStencilFunc(uint  func,int refx,uint  mask);
		[DllImport(GL_DLL,EntryPoint ="glStencilMask")]
		public static extern void glStencilMask(uint  mask);
		[DllImport(GL_DLL,EntryPoint ="glStencilOp")]
		public static extern void glStencilOp(uint  fail,uint  zfail,uint  zpass);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord1d")]
		public static extern void glTexCoord1d(double s);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord1dv")]
		public static extern void glTexCoord1dv(double[] v);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord1f")]
		public static extern void glTexCoord1f(float s);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord1fv")]
		public static extern void glTexCoord1fv(float[] v);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord1i")]
		public static extern void glTexCoord1i(int s);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord1iv")]
		public static extern void glTexCoord1iv(int[] v);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord1s")]
		public static extern void glTexCoord1s(short s);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord1sv")]
		public static extern void glTexCoord1sv(short[] v);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord2d")]
		public static extern void glTexCoord2d(double s,double t);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord2dv")]
		public static extern void glTexCoord2dv(double[] v);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord2f")]
		public static extern void glTexCoord2f(float s,float t);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord2fv")]
		public static extern void glTexCoord2fv(float[] v);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord2i")]
		public static extern void glTexCoord2i(int s,int t);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord2iv")]
		public static extern void glTexCoord2iv(int[] v);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord2s")]
		public static extern void glTexCoord2s(short s,short t);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord2sv")]
		public static extern void glTexCoord2sv(short[] v);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord3d")]
		public static extern void glTexCoord3d(double s,double t,double r);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord3dv")]
		public static extern void glTexCoord3dv(double[] v);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord3f")]
		public static extern void glTexCoord3f(float s,float t,float r);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord3fv")]
		public static extern void glTexCoord3fv(float[] v);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord3i")]
		public static extern void glTexCoord3i(int s,int t,int r);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord3iv")]
		public static extern void glTexCoord3iv(int[] v);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord3s")]
		public static extern void glTexCoord3s(short s,short t,short r);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord3sv")]
		public static extern void glTexCoord3sv(short[] v);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord4d")]
		public static extern void glTexCoord4d(double s,double t,double r,double q);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord4dv")]
		public static extern void glTexCoord4dv(double[] v);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord4f")]
		public static extern void glTexCoord4f(float s,float t,float r,float q);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord4fv")]
		public static extern void glTexCoord4fv(float[] v);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord4i")]
		public static extern void glTexCoord4i(int s,int t,int r,int q);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord4iv")]
		public static extern void glTexCoord4iv(int[] v);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord4s")]
		public static extern void glTexCoord4s(short s,short t,short r,short q);
		[DllImport(GL_DLL,EntryPoint ="glTexCoord4sv")]
		public static extern void glTexCoord4sv(short[] v);
		[DllImport(GL_DLL,EntryPoint ="glTexCoordPointer")]
		public static extern void glTexCoordPointer(int size,uint  type,int stride,object[] pointer);
		[DllImport(GL_DLL,EntryPoint ="glTexEnvf")]
		public static extern void glTexEnvf(uint  target,uint  pname,float param);
		[DllImport(GL_DLL,EntryPoint ="glTexEnvfv")]
		public static extern void glTexEnvfv(uint  target,uint  pname,float[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glTexEnvi")]
		public static extern void glTexEnvi(uint  target,uint  pname,int param);
		[DllImport(GL_DLL,EntryPoint ="glTexEnviv")]
		public static extern void glTexEnviv(uint  target,uint  pname,int[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glTexGend")]
		public static extern void glTexGend(uint  coord,uint  pname,double param);
		[DllImport(GL_DLL,EntryPoint ="glTexGendv")]
		public static extern void glTexGendv(uint  coord,uint  pname,double[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glTexGenf")]
		public static extern void glTexGenf(uint  coord,uint  pname,float param);
		[DllImport(GL_DLL,EntryPoint ="glTexGenfv")]
		public static extern void glTexGenfv(uint  coord,uint  pname,float[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glTexGeni")]
		public static extern void glTexGeni(uint  coord,uint  pname,int param);
		[DllImport(GL_DLL,EntryPoint ="glTexGeniv")]
		public static extern void glTexGeniv(uint  coord,uint  pname,int[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glTexImage1D")]
		public static extern void glTexImage1D(uint  target,int level,int internalformat,int width,int border,uint  format,uint  type,object[] pixels);
		[DllImport(GL_DLL,EntryPoint ="glTexImage2D")]
		public static extern void glTexImage2D(uint  target,int level,int internalformat,int width,int height,int border,uint  format,uint  type,object[] pixels);
		[DllImport(GL_DLL,EntryPoint ="glTexParameterf")]
		public static extern void glTexParameterf(uint  target,uint  pname,float param);
		[DllImport(GL_DLL,EntryPoint ="glTexParameterfv")]
		public static extern void glTexParameterfv(uint  target,uint  pname,float[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glTexParameteri")]
		public static extern void glTexParameteri(uint  target,uint  pname,int param);
		[DllImport(GL_DLL,EntryPoint ="glTexParameteriv")]
		public static extern void glTexParameteriv(uint  target,uint  pname,int[] paramsx);
		[DllImport(GL_DLL,EntryPoint ="glTexSubImage1D")]
		public static extern void glTexSubImage1D(uint  target,int level,int xoffset,int width,uint  format,uint  type,object[] pixels);
		[DllImport(GL_DLL,EntryPoint ="glTexSubImage2D")]
		public static extern void glTexSubImage2D(uint  target,int level,int xoffset,int yoffset,int width,int height,uint  format,uint  type,object[] pixels);
		[DllImport(GL_DLL,EntryPoint ="glTranslated")]
		public static extern void glTranslated(double x,double y,double z);
		[DllImport(GL_DLL,EntryPoint ="glTranslatef")]
		public static extern void glTranslatef(float x,float y,float z);
		[DllImport(GL_DLL,EntryPoint ="glVertex2d")]
		public static extern void glVertex2d(double x,double y);
		[DllImport(GL_DLL,EntryPoint ="glVertex2dv")]
		public static extern void glVertex2dv(double[] v);
		[DllImport(GL_DLL,EntryPoint ="glVertex2f")]
		public static extern void glVertex2f(float x,float y);
		[DllImport(GL_DLL,EntryPoint ="glVertex2fv")]
		public static extern void glVertex2fv(float[] v);
		[DllImport(GL_DLL,EntryPoint ="glVertex2i")]
		public static extern void glVertex2i(int x,int y);
		[DllImport(GL_DLL,EntryPoint ="glVertex2iv")]
		public static extern void glVertex2iv(int[] v);
		[DllImport(GL_DLL,EntryPoint ="glVertex2s")]
		public static extern void glVertex2s(short x,short y);
		[DllImport(GL_DLL,EntryPoint ="glVertex2sv")]
		public static extern void glVertex2sv(short[] v);
		[DllImport(GL_DLL,EntryPoint ="glVertex3d")]
		public static extern void glVertex3d(double x,double y,double z);
		[DllImport(GL_DLL,EntryPoint ="glVertex3dv")]
		public static extern void glVertex3dv(double[] v);
		[DllImport(GL_DLL,EntryPoint ="glVertex3f")]
		public static extern void glVertex3f(float x,float y,float z);
		[DllImport(GL_DLL,EntryPoint ="glVertex3fv")]
		public static extern void glVertex3fv(float[] v);
		[DllImport(GL_DLL,EntryPoint ="glVertex3i")]
		public static extern void glVertex3i(int x,int y,int z);
		[DllImport(GL_DLL,EntryPoint ="glVertex3iv")]
		public static extern void glVertex3iv(int[] v);
		[DllImport(GL_DLL,EntryPoint ="glVertex3s")]
		public static extern void glVertex3s(short x,short y,short z);
		[DllImport(GL_DLL,EntryPoint ="glVertex3sv")]
		public static extern void glVertex3sv(short[] v);
		[DllImport(GL_DLL,EntryPoint ="glVertex4d")]
		public static extern void glVertex4d(double x,double y,double z,double w);
		[DllImport(GL_DLL,EntryPoint ="glVertex4dv")]
		public static extern void glVertex4dv(double[] v);
		[DllImport(GL_DLL,EntryPoint ="glVertex4f")]
		public static extern void glVertex4f(float x,float y,float z,float w);
		[DllImport(GL_DLL,EntryPoint ="glVertex4fv")]
		public static extern void glVertex4fv(float[] v);
		[DllImport(GL_DLL,EntryPoint ="glVertex4i")]
		public static extern void glVertex4i(int x,int y,int z,int w);
		[DllImport(GL_DLL,EntryPoint ="glVertex4iv")]
		public static extern void glVertex4iv(int[] v);
		[DllImport(GL_DLL,EntryPoint ="glVertex4s")]
		public static extern void glVertex4s(short x,short y,short z,short w);
		[DllImport(GL_DLL,EntryPoint ="glVertex4sv")]
		public static extern void glVertex4sv(short[] v);
		[DllImport(GL_DLL,EntryPoint ="glVertexPointer")]
		public static extern void glVertexPointer(int size,uint  type,int stride,object[] pointer);
		[DllImport(GL_DLL,EntryPoint ="glViewport")]
		public static extern void glViewport(int x,int y,int width,int height);

        #endregion

        #region [ GLU    ]
		
        public const string GLU_DLL ="glu32";

		/**** Generic constants****/

		/* Version */
		public const uint GLU_VERSION_1_1 = 1;
		public const uint GLU_VERSION_1_2 = 1;

		/* Errors: (return value 0 = no error) */
		public const uint GLU_INVALID_ENUM = 100900;
		public const uint GLU_INVALID_VALUE= 100901;
		public const uint GLU_OUT_OF_MEMORY= 100902;
		public const uint GLU_INCOMPATIBLE_GL_VERSION= 100903;

		/* StringName */
		public const uint GLU_VERSION = 100800;
		public const uint GLU_EXTENSIONS  = 100801;

		/* Boolean */
		public const uint GLU_TRUE  = 1; // GL_TRUE;
		public const uint GLU_FALSE  = 0; // GL_FALSE;

		/**** Quadric constants****/

		/* QuadricNormal */
		public const uint GLU_SMOOTH = 100000;
		public const uint GLU_FLAT  = 100001;
		public const uint GLU_NONE  = 100002;

		/* QuadricDrawStyle */
		public const uint GLU_POINT  = 100010;
		public const uint GLU_LINE  = 100011;
		public const uint GLU_FILL  = 100012;
		public const uint GLU_SILHOUETTE  = 100013;

		/* QuadricOrientation */
		public const uint GLU_OUTSIDE = 100020;
		public const uint GLU_INSIDE = 100021;

		/* Callback types: */
		/* GLU_ERROR100103 */

		/**** Tesselation constants ****/

		public const double GLU_TESS_MAX_COORD  = 1.0e150;

		/* TessProperty */
		public const uint GLU_TESS_WINDING_RULE = 100140;
		public const uint GLU_TESS_BOUNDARY_ONLY= 100141;
		public const uint GLU_TESS_TOLERANCE  = 100142;

		/* TessWinding */
		public const uint GLU_TESS_WINDING_ODD = 100130;
		public const uint GLU_TESS_WINDING_NONZERO  = 100131;
		public const uint GLU_TESS_WINDING_POSITIVE = 100132;
		public const uint GLU_TESS_WINDING_NEGATIVE = 100133;
		public const uint GLU_TESS_WINDING_ABS_GEQ_TWO  = 100134;

		/* TessCallback */
		public const uint GLU_TESS_BEGIN  = 100100;/* void (CALLBACK*)(GLenum  type) */
		public const uint GLU_TESS_VERTEX = 100101;/* void (CALLBACK*)(void *data) */
		public const uint GLU_TESS_END= 100102;/* void (CALLBACK*)(void) */
		public const uint GLU_TESS_ERROR  = 100103;/* void (CALLBACK*)(GLenum  errno) */
		public const uint GLU_TESS_EDGE_FLAG  = 100104;/* void (CALLBACK*)(GLboolean boundaryEdge) */
		public const uint GLU_TESS_COMBINE = 100105;/* void (CALLBACK*)(GLdouble coords[3],
														void *data[4],
														GLfloat  weight[4],
														void **dataOut)*/
		public const uint GLU_TESS_BEGIN_DATA  = 100106;/* void (CALLBACK*)(GLenum  type, 
														void *polygon_data) */
		public const uint GLU_TESS_VERTEX_DATA = 100107;/* void (CALLBACK*)(void *data, 
														void *polygon_data) */
		public const uint GLU_TESS_END_DATA= 100108;/* void (CALLBACK*)(void *polygon_data) */
		public const uint GLU_TESS_ERROR_DATA  = 100109;/* void (CALLBACK*)(GLenum  errno, 
														void *polygon_data) */
		public const uint GLU_TESS_EDGE_FLAG_DATA  = 100110;/* void (CALLBACK*)(GLboolean boundaryEdge,
														void *polygon_data) */
		public const uint GLU_TESS_COMBINE_DATA = 100111;/* void (CALLBACK*)(GLdouble coords[3],
														void *data[4],
														GLfloat  weight[4],
														void **dataOut,
														void *polygon_data) */

		/* TessError */
		public const uint GLU_TESS_ERROR1 = 100151;
		public const uint GLU_TESS_ERROR2 = 100152;
		public const uint GLU_TESS_ERROR3 = 100153;
		public const uint GLU_TESS_ERROR4 = 100154;
		public const uint GLU_TESS_ERROR5 = 100155;
		public const uint GLU_TESS_ERROR6 = 100156;
		public const uint GLU_TESS_ERROR7 = 100157;
		public const uint GLU_TESS_ERROR8 = 100158;

		public const uint GLU_TESS_MISSING_BEGIN_POLYGON = GLU_TESS_ERROR1;
		public const uint GLU_TESS_MISSING_BEGIN_CONTOUR = GLU_TESS_ERROR2;
		public const uint GLU_TESS_MISSING_END_POLYGON  = GLU_TESS_ERROR3;
		public const uint GLU_TESS_MISSING_END_CONTOUR  = GLU_TESS_ERROR4;
		public const uint GLU_TESS_COORD_TOO_LARGE  = GLU_TESS_ERROR5;
		public const uint GLU_TESS_NEED_COMBINE_CALLBACK = GLU_TESS_ERROR6;

		/**** NURBS constants ****/

		/* NurbsProperty */
		public const uint GLU_AUTO_LOAD_MATRIX = 100200;
		public const uint GLU_CULLING = 100201;
		public const uint GLU_SAMPLING_TOLERANCE= 100203;
		public const uint GLU_DISPLAY_MODE = 100204;
		public const uint GLU_PARAMETRIC_TOLERANCE  = 100202;
		public const uint GLU_SAMPLING_METHOD  = 100205;
		public const uint GLU_U_STEP = 100206;
		public const uint GLU_V_STEP = 100207;

		/* NurbsSampling */
		public const uint GLU_PATH_LENGTH = 100215;
		public const uint GLU_PARAMETRIC_ERROR = 100216;
		public const uint GLU_DOMAIN_DISTANCE  = 100217;

		/* NurbsTrim */
		public const uint GLU_MAP1_TRIM_2 = 100210;
		public const uint GLU_MAP1_TRIM_3 = 100211;

		/* NurbsDisplay */
		/* GLU_FILL 100012 */
		public const uint GLU_OUTLINE_POLYGON  = 100240;
		public const uint GLU_OUTLINE_PATCH= 100241;

		/* NurbsCallback */
		/* GLU_ERROR100103 */

		/* NurbsErrors */
		public const uint GLU_NURBS_ERROR1 = 100251;
		public const uint GLU_NURBS_ERROR2 = 100252;
		public const uint GLU_NURBS_ERROR3 = 100253;
		public const uint GLU_NURBS_ERROR4 = 100254;
		public const uint GLU_NURBS_ERROR5 = 100255;
		public const uint GLU_NURBS_ERROR6 = 100256;
		public const uint GLU_NURBS_ERROR7 = 100257;
		public const uint GLU_NURBS_ERROR8 = 100258;
		public const uint GLU_NURBS_ERROR9 = 100259;
		public const uint GLU_NURBS_ERROR10= 100260;
		public const uint GLU_NURBS_ERROR11= 100261;
		public const uint GLU_NURBS_ERROR12= 100262;
		public const uint GLU_NURBS_ERROR13= 100263;
		public const uint GLU_NURBS_ERROR14= 100264;
		public const uint GLU_NURBS_ERROR15= 100265;
		public const uint GLU_NURBS_ERROR16= 100266;
		public const uint GLU_NURBS_ERROR17= 100267;
		public const uint GLU_NURBS_ERROR18= 100268;
		public const uint GLU_NURBS_ERROR19= 100269;
		public const uint GLU_NURBS_ERROR20= 100270;
		public const uint GLU_NURBS_ERROR21= 100271;
		public const uint GLU_NURBS_ERROR22= 100272;
		public const uint GLU_NURBS_ERROR23= 100273;
		public const uint GLU_NURBS_ERROR24= 100274;
		public const uint GLU_NURBS_ERROR25= 100275;
		public const uint GLU_NURBS_ERROR26= 100276;
		public const uint GLU_NURBS_ERROR27= 100277;
		public const uint GLU_NURBS_ERROR28= 100278;
		public const uint GLU_NURBS_ERROR29= 100279;
		public const uint GLU_NURBS_ERROR30= 100280;
		public const uint GLU_NURBS_ERROR31= 100281;
		public const uint GLU_NURBS_ERROR32= 100282;
		public const uint GLU_NURBS_ERROR33= 100283;
		public const uint GLU_NURBS_ERROR34= 100284;
		public const uint GLU_NURBS_ERROR35= 100285;
		public const uint GLU_NURBS_ERROR36= 100286;
		public const uint GLU_NURBS_ERROR37= 100287;

		/**** Backwards compatibility for old tesselator ****/

		/* Contours types -- obsolete! */
		public const uint GLU_CW = 100120;
		public const uint GLU_CCW= 100121;
		public const uint GLU_INTERIOR= 100122;
		public const uint GLU_EXTERIOR= 100123;
		public const uint GLU_UNKNOWN = 100124;

		/* Names without"TESS_"prefix */
		public const uint GLU_BEGIN  = GLU_TESS_BEGIN;
		public const uint GLU_VERTEX = GLU_TESS_VERTEX;
		public const uint GLU_END= GLU_TESS_END;
		public const uint GLU_ERROR  = GLU_TESS_ERROR;
		public const uint GLU_EDGE_FLAG  = GLU_TESS_EDGE_FLAG;


		[DllImport(GLU_DLL, EntryPoint ="gluErrorString")]
		public static extern byte[] gluErrorString ( uint errCode );
		[DllImport(GLU_DLL, EntryPoint ="gluGetString")]
		public static extern byte[] gluGetString  ( uint name );
		[DllImport(GLU_DLL, EntryPoint ="gluOrtho2D")]
		public static extern void  gluOrtho2D( double left, double right, double bottom, double top );
		[DllImport(GLU_DLL, EntryPoint ="gluPerspective")]
		public static extern void  gluPerspective ( double fovy, double aspect, double zNear, double zFar );
		[DllImport(GLU_DLL, EntryPoint ="gluPickMatrix")]
		public static extern void  gluPickMatrix ( double x, double y, double width, double height, int viewport );
		[DllImport(GLU_DLL, EntryPoint ="gluLookAt")]
		public static extern void  gluLookAt ( double eyex, double eyey, double eyez, double centerx, double centery, double centerz, double upx, double upy, double upz );
		[DllImport(GLU_DLL, EntryPoint ="gluProject")]
		public static extern int  gluProject( double objx, double objy, double objz, double modelMatrix, double projMatrix, int viewport, ref double winx, ref double winy, ref double winz );
		[DllImport(GLU_DLL, EntryPoint ="gluUnProject")]
		public static extern int  gluUnProject  ( double winx, double winy, double winz, double modelMatrix, double projMatrix, int viewport, ref double objx, ref double objy, ref double objz );
		[DllImport(GLU_DLL, EntryPoint ="gluScaleImage")]
		public static extern int  gluScaleImage ( uint format, int widthin, int heightin, uint typein, object[] datain, int widthout, int heightout, uint typeout, object[] dataout );
		[DllImport(GLU_DLL, EntryPoint ="gluBuild1DMipmaps")]
		public static extern int  gluBuild1DMipmaps  ( uint target, int components, int width, uint format, uint type, object[] data );
		[DllImport(GLU_DLL, EntryPoint ="gluBuild2DMipmaps")]
		public static extern int  gluBuild2DMipmaps  ( uint target, int components, int width, int height, uint format, uint type, object[] data );
		[DllImport(GLU_DLL, EntryPoint ="gluNewQuadric")]
		public static extern IntPtr gluNewQuadric ( );
		[DllImport(GLU_DLL, EntryPoint ="gluDeleteQuadric")]
		public static extern IntPtr  gluDeleteQuadric  (  IntPtr state );
		[DllImport(GLU_DLL, EntryPoint ="gluQuadricNormals")]
		public static extern void  gluQuadricNormals  ( ref object quadObject, uint normals );
		[DllImport(GLU_DLL, EntryPoint ="gluQuadricTexture")]
		public static extern void  gluQuadricTexture  ( ref object quadObject, byte textureCoords );
		[DllImport(GLU_DLL, EntryPoint ="gluQuadricOrientation")]
		public static extern void  gluQuadricOrientation  ( ref object quadObject, uint orientation );
		[DllImport(GLU_DLL, EntryPoint ="gluQuadricDrawStyle")]
		public static extern IntPtr  gluQuadricDrawStyle ( IntPtr quadObject, uint drawStyle );
		[DllImport(GLU_DLL, EntryPoint ="gluCylinder")]
		public static extern IntPtr  gluCylinder  ( IntPtr qobj, double baseRadius, double topRadius, double height, int slices, int stacks );
		[DllImport(GLU_DLL, EntryPoint ="gluDisk")]
		public static extern void  gluDisk  ( ref object qobj, double innerRadius, double outerRadius, int slices, int loops );
		[DllImport(GLU_DLL, EntryPoint ="gluPartialDisk")]
		public static extern void  gluPartialDisk ( ref object qobj, double innerRadius, double outerRadius, int slices, int loops, double startAngle, double sweepAngle );
		[DllImport(GLU_DLL, EntryPoint ="gluSphere")]
		public static extern IntPtr  gluSphere (  IntPtr qobj, double radius, int slices, int stacks );
		[DllImport(GLU_DLL, EntryPoint ="gluQuadricCallback")]
		public static extern void  gluQuadricCallback ( ref object qobj, uint which, IntPtr fn );
		[DllImport(GLU_DLL, EntryPoint ="gluNewTess")]
		public static extern IntPtr gluNewTess( );
		[DllImport(GLU_DLL, EntryPoint ="gluDeleteTess")]
		public static extern void  gluDeleteTess ( IntPtr tess );
		[DllImport(GLU_DLL, EntryPoint ="gluTessBeginPolygon")]
		public static extern void  gluTessBeginPolygon ( IntPtr tess, object polygon_data );
		[DllImport(GLU_DLL, EntryPoint ="gluTessBeginContour")]
		public static extern void  gluTessBeginContour ( IntPtr tess );
		[DllImport(GLU_DLL, EntryPoint ="gluTessVertex")]
		public static extern void  gluTessVertex ( IntPtr tess, double coords, object data );
		[DllImport(GLU_DLL, EntryPoint ="gluTessEndContour")]
		public static extern void  gluTessEndContour  ( IntPtr tess );
		[DllImport(GLU_DLL, EntryPoint ="gluTessEndPolygon")]
		public static extern void  gluTessEndPolygon  ( IntPtr tess );
		[DllImport(GLU_DLL, EntryPoint ="gluTessProperty")]
		public static extern void  gluTessProperty( IntPtr tess, uint which, double valuex );
		[DllImport(GLU_DLL, EntryPoint ="gluTessNormal")]
		public static extern void  gluTessNormal ( IntPtr tess, double x, double y, double z );
		[DllImport(GLU_DLL, EntryPoint ="gluTessCallback")]
		public static extern void  gluTessCallback( IntPtr tess, uint which, IntPtr fn );
		[DllImport(GLU_DLL, EntryPoint ="gluGetTessProperty")]
		public static extern void  gluGetTessProperty ( IntPtr tess, uint which, ref double valuex );
		[DllImport(GLU_DLL, EntryPoint ="gluNewNurbsRenderer")]
		public static extern IntPtr gluNewNurbsRenderer ( );
		[DllImport(GLU_DLL, EntryPoint ="gluDeleteNurbsRenderer")]
		public static extern void  gluDeleteNurbsRenderer  ( IntPtr nobj );
		[DllImport(GLU_DLL, EntryPoint ="gluBeginSurface")]
		public static extern void  gluBeginSurface( IntPtr nobj );
		[DllImport(GLU_DLL, EntryPoint ="gluBeginCurve")]
		public static extern void  gluBeginCurve ( IntPtr nobj );
		[DllImport(GLU_DLL, EntryPoint ="gluEndCurve")]
		public static extern void  gluEndCurve  ( IntPtr nobj );
		[DllImport(GLU_DLL, EntryPoint ="gluEndSurface")]
		public static extern void  gluEndSurface ( IntPtr nobj );
		[DllImport(GLU_DLL, EntryPoint ="gluBeginTrim")]
		public static extern void  gluBeginTrim  ( IntPtr nobj );
		[DllImport(GLU_DLL, EntryPoint ="gluEndTrim")]
		public static extern void  gluEndTrim( IntPtr nobj );
		[DllImport(GLU_DLL, EntryPoint ="gluPwlCurve")]
		public static extern void  gluPwlCurve  ( IntPtr nobj, int count, float[] array, int stride, uint type );
		[DllImport(GLU_DLL, EntryPoint ="gluNurbsCurve")]
		public static extern void  gluNurbsCurve ( IntPtr nobj, int nknots, float[] knot, int stride, float[] ctlarray, int order, uint type );
		[DllImport(GLU_DLL, EntryPoint ="gluNurbsSurface")]
		public static extern void  gluNurbsSurface( IntPtr nobj, int sknot_count, float[] sknot, int tknot_count, float[] tknot, int s_stride, int t_stride, float[] ctlarray, int sorder, int torder, uint type );
		[DllImport(GLU_DLL, EntryPoint ="gluLoadSamplingMatrices")]
		public static extern void  gluLoadSamplingMatrices ( IntPtr nobj, float modelMatrix, float projMatrix, int viewport );
		[DllImport(GLU_DLL, EntryPoint ="gluNurbsProperty")]
		public static extern void  gluNurbsProperty  ( IntPtr nobj, uint property, float valuex );
		[DllImport(GLU_DLL, EntryPoint ="gluGetNurbsProperty")]
		public static extern void  gluGetNurbsProperty ( IntPtr nobj, uint property, float[] valuex );
		[DllImport(GLU_DLL, EntryPoint ="gluNurbsCallback")]
		public static extern void  gluNurbsCallback  ( IntPtr nobj, uint which, IntPtr fn );
		[DllImport(GLU_DLL, EntryPoint ="gluBeginPolygon")]
		public static extern void  gluBeginPolygon( IntPtr tess );
		[DllImport(GLU_DLL, EntryPoint ="gluNextContour")]
		public static extern void  gluNextContour ( IntPtr tess, uint type );
		[DllImport(GLU_DLL, EntryPoint ="gluEndPolygon")]
		public static extern void  gluEndPolygon ( IntPtr tess );

        #endregion



    }
}
