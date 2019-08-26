using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrHello.NetLib.Core.Windows.Windows
{
    public class KeyBoardApi
    {
        internal KeyBoardApi()
        {
        }

        private const byte KeyDownFlag = 0;
        private const byte KeyUpFlag = 2;

        /// <summary>
        /// 鼠标按压持续时间
        /// </summary>
        private const uint KeyPressedTime = 800;

        /// <summary>
        /// keybd_event
        /// </summary>
        /// <param name="bVk">按键的虚拟键值</param>
        /// <param name="bScan">扫描码，一般不用设置，用0代替就行</param>
        /// <param name="dwFlags">选项标志：0：表示按下，2：表示松开</param>
        /// <param name="dwExtraInfo">一般设置为0</param>
        [DllImport("user32.dll")]
        // ReSharper disable once IdentifierTypo
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, IntPtr dwExtraInfo);

        /// <summary>
        /// KeyBoard click
        /// </summary>
        /// <param name="keyName">key name</param>
        public void KeyClick(KeyBoardDefine keyName)
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            keybd_event(keyName.GetByte(), 0, KeyDownFlag, IntPtr.Zero);
            keybd_event(keyName.GetByte(), 0, KeyUpFlag, IntPtr.Zero);

            WindowsApi.WriteLog($"{nameof(KeyClick)} {keyName}");
        }

        /// <summary>
        /// KeyBoard click
        /// </summary>
        /// <param name="keyNames">key names</param>
        public void KeyClick(params KeyBoardDefine[] keyNames)
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            foreach (var keyName in keyNames)
            {
                keybd_event(keyName.GetByte(), 0, KeyDownFlag, IntPtr.Zero);
            }

            foreach (var keyName in keyNames)
            {
                keybd_event(keyName.GetByte(), 0, KeyUpFlag, IntPtr.Zero);
                WindowsApi.WriteLog($"{nameof(KeyClick)} {keyName}");
            }
        }

        /// <summary>
        /// KeyBoard down
        /// </summary>
        /// <param name="keyName">key name</param>
        public void KeyDown(KeyBoardDefine keyName)
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            keybd_event(keyName.GetByte(), 0, KeyDownFlag, IntPtr.Zero);
            WindowsApi.WriteLog($"{nameof(KeyDown)} {keyName}");
        }

        /// <summary>
        /// KeyBoard down
        /// </summary>
        /// <param name="keyNames">key names</param>
        public void KeyDown(params KeyBoardDefine[] keyNames)
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            foreach (var keyName in keyNames)
            {
                keybd_event(keyName.GetByte(), 0, KeyDownFlag, IntPtr.Zero);
                WindowsApi.WriteLog($"{nameof(KeyDown)} {keyName}");
            }
        }

        /// <summary>
        /// KeyBoard up
        /// </summary>
        /// <param name="keyName">key name</param>
        public void KeyUp(KeyBoardDefine keyName)
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            keybd_event(keyName.GetByte(), 0, KeyUpFlag, IntPtr.Zero);
            WindowsApi.WriteLog($"{nameof(KeyUp)} {keyName}");
        }

        /// <summary>
        /// KeyBoard up
        /// </summary>
        /// <param name="keyNames">key names</param>
        public void KeyUp(params KeyBoardDefine[] keyNames)
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            foreach (var keyName in keyNames)
            {
                keybd_event(keyName.GetByte(), 0, KeyUpFlag, IntPtr.Zero);
                WindowsApi.WriteLog($"{nameof(KeyUp)} {keyName}");
            }
        }

        /// <summary>
        /// KeyPress
        /// </summary>
        /// <param name="keyName">key name</param>
        /// <param name="pressedMillionSeconds">pressed time</param>
        /// <returns></returns>
        public static async Task KeyPress(KeyBoardDefine keyName, uint pressedMillionSeconds = KeyPressedTime)
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            keybd_event(keyName.GetByte(), 0, KeyDownFlag, IntPtr.Zero);

            if (pressedMillionSeconds != 0u)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(pressedMillionSeconds));
            }

            keybd_event(keyName.GetByte(), 0, KeyUpFlag, IntPtr.Zero);
            WindowsApi.WriteLog($"{nameof(KeyPress)} {keyName} {nameof(KeyPressedTime)}:{pressedMillionSeconds}");
        }

        /// <summary>
        /// KeyPress
        /// </summary>
        /// <param name="pressedMillionSeconds">pressed time</param>
        /// <param name="keyNames">key names</param>
        /// <returns></returns>
        public static async Task KeyPress(uint pressedMillionSeconds = KeyPressedTime, params KeyBoardDefine[] keyNames)
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            foreach (var keyName in keyNames)
            {
                keybd_event(keyName.GetByte(), 0, KeyDownFlag, IntPtr.Zero);
            }

            if (pressedMillionSeconds != 0u)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(pressedMillionSeconds));
            }

            foreach (var keyName in keyNames)
            {
                keybd_event(keyName.GetByte(), 0, KeyUpFlag, IntPtr.Zero);
                WindowsApi.WriteLog($"{nameof(KeyPress)} {keyName} {nameof(KeyPressedTime)}:{pressedMillionSeconds}");
            }
        }

        /// <summary>
        /// Input text
        /// </summary>
        /// <param name="text">text</param>
        public void InputString(string text)
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            if (text != null)
            {
                SendKeys.SendWait(text);
                WindowsApi.WriteLog($"{nameof(InputString)} {nameof(text)} is \"{text}\".");
            }
            else
            {
                WindowsApi.WriteLog($"{nameof(InputString)} {nameof(text)} is null.");
            }
        }
    }

    /// <summary>
    /// KeyBoard Keys
    /// </summary>
    public enum KeyBoardDefine
    {
        /// <summary>
        /// 鼠标左键
        /// </summary>
        KeyLButton,

        /// <summary>
        /// 鼠标右键
        /// </summary>
        KeyRButton,

        /// <summary>
        /// CANCEL 键
        /// </summary>
        KeyCancel,

        /// <summary>
        /// 鼠标中键
        /// </summary>
        KeyMButton,

        /// <summary>
        /// BACKSPACE 键
        /// </summary>
        KeyBack,

        /// <summary>
        /// TAB 键
        /// </summary>
        KeyTab,

        /// <summary>
        /// CLEAR 键
        /// </summary>
        KeyClear,

        /// <summary>
        /// ENTER 键
        /// </summary>
        KeyReturn,

        /// <summary>
        /// SHIFT 键
        /// </summary>
        KeyShift,

        /// <summary>
        /// CTRL 键
        /// </summary>
        KeyControl,

        /// <summary>
        /// Alt 键
        /// </summary>
        KeyAlt,

        /// <summary>
        /// MENU 键
        /// </summary>
        KeyMenu,

        /// <summary>
        /// PAUSE 键
        /// </summary>
        KeyPause,

        /// <summary>
        /// CAPS LOCK 键
        /// </summary>
        KeyCapital,

        /// <summary>
        /// ESC 键
        /// </summary>
        KeyEscape,

        /// <summary>
        /// SPACEBAR 键
        /// </summary>
        KeySpace,

        /// <summary>
        /// PAGE UP 键
        /// </summary>
        KeyPageUp,

        /// <summary>
        /// End 键
        /// </summary>
        KeyEnd,

        /// <summary>
        /// HOME 键
        /// </summary>
        KeyHome,

        /// <summary>
        /// LEFT ARROW 键
        /// </summary>
        KeyLeft,

        /// <summary>
        /// UP ARROW 键
        /// </summary>
        KeyUp,

        /// <summary>
        /// RIGHT ARROW 键
        /// </summary>
        KeyRight,

        /// <summary>
        /// DOWN ARROW 键
        /// </summary>
        KeyDown,

        /// <summary>
        /// Select 键
        /// </summary>
        KeySelect,

        /// <summary>
        /// PRINT SCREEN 键
        /// </summary>
        KeyPrint,

        /// <summary>
        /// EXECUTE 键
        /// </summary>
        KeyExecute,

        /// <summary>
        /// SNAPSHOT 键
        /// </summary>
        KeySnapshot,

        /// <summary>
        /// Delete 键
        /// </summary>
        KeyDelete,

        /// <summary>
        /// HELP 键
        /// </summary>
        KeyHelp,

        /// <summary>
        /// NUM LOCK 键
        /// </summary>
        KeyNumlock,

        //常用键 字母键A到Z
        KeyA,
        KeyB,
        KeyC,
        KeyD,
        KeyE,
        KeyF,
        KeyG,
        KeyH,
        KeyI,
        KeyJ,
        KeyK,
        KeyL,
        KeyM,
        KeyN,
        KeyO,
        KeyP,
        KeyQ,
        KeyR,
        KeyS,
        KeyT,
        KeyU,
        KeyV,
        KeyW,
        KeyX,
        KeyY,
        KeyZ,

        //数字键盘0到9
        Key0, // 0 键
        Key1, // 1 键
        Key2, // 2 键
        Key3, // 3 键
        Key4, // 4 键
        Key5, // 5 键
        Key6, // 6 键
        Key7, // 7 键
        Key8, // 8 键
        Key9, // 9 键

        KeyNumpad0, //0 键
        KeyNumpad1, //1 键
        KeyNumpad2, //2 键
        KeyNumpad3, //3 键
        KeyNumpad4, //4 键
        KeyNumpad5, //5 键
        KeyNumpad6, //6 键
        KeyNumpad7, //7 键
        KeyNumpad8, //8 键
        KeyNumpad9, //9 键
        KeyMultiply, // MULTIPLICATIONSIGN(*)键
        KeyAdd, // PLUS SIGN(+) 键
        KeySeparator, // ENTER 键
        KeySubtract, // MINUS SIGN(-) 键
        KeyDecimal, // DECIMAL POINT(.) 键
        KeyDivide, // DIVISION SIGN(/) 键

        //F1到F12按键
        KeyF1, //F1 键
        KeyF2, //F2 键
        KeyF3, //F3 键
        KeyF4, //F4 键
        KeyF5, //F5 键
        KeyF6, //F6 键
        KeyF7, //F7 键
        KeyF8, //F8 键
        KeyF9, //F9 键
        KeyF10, //F10 键
        KeyF11, //F11 键
        KeyF12 //F12 键
    }

    /// <summary>
    /// KeyBoardExtension
    /// </summary>
    internal static class KeyBoardExtension
    {
        private const byte KeyLButton = 0x1; // 鼠标左键
        private const byte KeyRButton = 0x2; // 鼠标右键
        private const byte KeyCancel = 0x3; // CANCEL 键
        private const byte KeyMButton = 0x4; // 鼠标中键
        private const byte KeyBack = 0x8; // BACKSPACE 键
        private const byte KeyTab = 0x9; // TAB 键
        private const byte KeyClear = 0xC; // CLEAR 键
        private const byte KeyReturn = 0xD; // ENTER 键
        private const byte KeyShift = 0x10; // SHIFT 键
        private const byte KeyControl = 0x11; // CTRL 键
        private const byte KeyAlt = 18; // Alt 键  (键码18)
        private const byte KeyMenu = 0x12; // MENU 键
        private const byte KeyPause = 0x13; // PAUSE 键
        private const byte KeyCapital = 0x14; // CAPS LOCK 键
        private const byte KeyEscape = 0x1B; // ESC 键
        private const byte KeySpace = 0x20; // SPACEBAR 键
        private const byte KeyPageUp = 0x21; // PAGE UP 键
        private const byte KeyEnd = 0x23; // End 键
        private const byte KeyHome = 0x24; // HOME 键
        private const byte KeyLeft = 0x25; // LEFT ARROW 键
        private const byte KeyUp = 0x26; // UP ARROW 键
        private const byte KeyRight = 0x27; // RIGHT ARROW 键
        private const byte KeyDown = 0x28; // DOWN ARROW 键
        private const byte KeySelect = 0x29; // Select 键
        private const byte KeyPrint = 0x2A; // PRINT SCREEN 键
        private const byte KeyExecute = 0x2B; // EXECUTE 键
        private const byte KeySnapshot = 0x2C; // SNAPSHOT 键
        private const byte KeyDelete = 0x2E; // Delete 键
        private const byte KeyHelp = 0x2F; // HELP 键
        private const byte KeyNumlock = 0x90; // NUM LOCK 键

        //常用键 字母键A到Z
        private const byte KeyA = 65;
        private const byte KeyB = 66;
        private const byte KeyC = 67;
        private const byte KeyD = 68;
        private const byte KeyE = 69;
        private const byte KeyF = 70;
        private const byte KeyG = 71;
        private const byte KeyH = 72;
        private const byte KeyI = 73;
        private const byte KeyJ = 74;
        private const byte KeyK = 75;
        private const byte KeyL = 76;
        private const byte KeyM = 77;
        private const byte KeyN = 78;
        private const byte KeyO = 79;
        private const byte KeyP = 80;
        private const byte KeyQ = 81;
        private const byte KeyR = 82;
        private const byte KeyS = 83;
        private const byte KeyT = 84;
        private const byte KeyU = 85;
        private const byte KeyV = 86;
        private const byte KeyW = 87;
        private const byte KeyX = 88;
        private const byte KeyY = 89;
        private const byte KeyZ = 90;

        //数字键盘0到9
        private const byte Key0 = 48; // 0 键
        private const byte Key1 = 49; // 1 键
        private const byte Key2 = 50; // 2 键
        private const byte Key3 = 51; // 3 键
        private const byte Key4 = 52; // 4 键
        private const byte Key5 = 53; // 5 键
        private const byte Key6 = 54; // 6 键
        private const byte Key7 = 55; // 7 键
        private const byte Key8 = 56; // 8 键
        private const byte Key9 = 57; // 9 键


        private const byte KeyNumpad0 = 0x60; //0 键
        private const byte KeyNumpad1 = 0x61; //1 键
        private const byte KeyNumpad2 = 0x62; //2 键
        private const byte KeyNumpad3 = 0x63; //3 键
        private const byte KeyNumpad4 = 0x64; //4 键
        private const byte KeyNumpad5 = 0x65; //5 键
        private const byte KeyNumpad6 = 0x66; //6 键
        private const byte KeyNumpad7 = 0x67; //7 键
        private const byte KeyNumpad8 = 0x68; //8 键
        private const byte KeyNumpad9 = 0x69; //9 键
        private const byte KeyMultiply = 0x6A; // MULTIPLICATIONSIGN(*)键
        private const byte KeyAdd = 0x6B; // PLUS SIGN(+) 键
        private const byte KeySeparator = 0x6C; // ENTER 键
        private const byte KeySubtract = 0x6D; // MINUS SIGN(-) 键
        private const byte KeyDecimal = 0x6E; // DECIMAL POINT(.) 键
        private const byte KeyDivide = 0x6F; // DIVISION SIGN(/) 键


        //F1到F12按键
        private const byte KeyF1 = 0x70; //F1 键
        private const byte KeyF2 = 0x71; //F2 键
        private const byte KeyF3 = 0x72; //F3 键
        private const byte KeyF4 = 0x73; //F4 键
        private const byte KeyF5 = 0x74; //F5 键
        private const byte KeyF6 = 0x75; //F6 键
        private const byte KeyF7 = 0x76; //F7 键
        private const byte KeyF8 = 0x77; //F8 键
        private const byte KeyF9 = 0x78; //F9 键
        private const byte KeyF10 = 0x79; //F10 键
        private const byte KeyF11 = 0x7A; //F11 键
        private const byte KeyF12 = 0x7B; //F12 键

        internal static byte GetByte(this KeyBoardDefine keyBoardDefine)
        {
            switch (keyBoardDefine)
            {
                case KeyBoardDefine.KeyLButton:
                    return KeyLButton;
                case KeyBoardDefine.KeyRButton:
                    return KeyRButton;
                case KeyBoardDefine.KeyCancel:
                    return KeyCancel;
                case KeyBoardDefine.KeyMButton:
                    return KeyMButton;
                case KeyBoardDefine.KeyBack:
                    return KeyBack;
                case KeyBoardDefine.KeyTab:
                    return KeyTab;
                case KeyBoardDefine.KeyClear:
                    return KeyClear;
                case KeyBoardDefine.KeyReturn:
                    return KeyReturn;
                case KeyBoardDefine.KeyShift:
                    return KeyShift;
                case KeyBoardDefine.KeyControl:
                    return KeyControl;
                case KeyBoardDefine.KeyAlt:
                    return KeyAlt;
                case KeyBoardDefine.KeyMenu:
                    return KeyMenu;
                case KeyBoardDefine.KeyPause:
                    return KeyPause;
                case KeyBoardDefine.KeyCapital:
                    return KeyCapital;
                case KeyBoardDefine.KeyEscape:
                    return KeyEscape;
                case KeyBoardDefine.KeySpace:
                    return KeySpace;
                case KeyBoardDefine.KeyPageUp:
                    return KeyPageUp;
                case KeyBoardDefine.KeyEnd:
                    return KeyEnd;
                case KeyBoardDefine.KeyHome:
                    return KeyHome;
                case KeyBoardDefine.KeyLeft:
                    return KeyLeft;
                case KeyBoardDefine.KeyUp:
                    return KeyUp;
                case KeyBoardDefine.KeyRight:
                    return KeyRight;
                case KeyBoardDefine.KeyDown:
                    return KeyDown;
                case KeyBoardDefine.KeySelect:
                    return KeySelect;
                case KeyBoardDefine.KeyPrint:
                    return KeyPrint;
                case KeyBoardDefine.KeyExecute:
                    return KeyExecute;
                case KeyBoardDefine.KeySnapshot:
                    return KeySnapshot;
                case KeyBoardDefine.KeyDelete:
                    return KeyDelete;
                case KeyBoardDefine.KeyHelp:
                    return KeyHelp;
                case KeyBoardDefine.KeyNumlock:
                    return KeyNumlock;
                case KeyBoardDefine.KeyA:
                    return KeyA;
                case KeyBoardDefine.KeyB:
                    return KeyB;
                case KeyBoardDefine.KeyC:
                    return KeyC;
                case KeyBoardDefine.KeyD:
                    return KeyD;
                case KeyBoardDefine.KeyE:
                    return KeyE;
                case KeyBoardDefine.KeyF:
                    return KeyF;
                case KeyBoardDefine.KeyG:
                    return KeyG;
                case KeyBoardDefine.KeyH:
                    return KeyH;
                case KeyBoardDefine.KeyI:
                    return KeyI;
                case KeyBoardDefine.KeyJ:
                    return KeyJ;
                case KeyBoardDefine.KeyK:
                    return KeyK;
                case KeyBoardDefine.KeyL:
                    return KeyL;
                case KeyBoardDefine.KeyM:
                    return KeyM;
                case KeyBoardDefine.KeyN:
                    return KeyN;
                case KeyBoardDefine.KeyO:
                    return KeyO;
                case KeyBoardDefine.KeyP:
                    return KeyP;
                case KeyBoardDefine.KeyQ:
                    return KeyQ;
                case KeyBoardDefine.KeyR:
                    return KeyR;
                case KeyBoardDefine.KeyS:
                    return KeyS;
                case KeyBoardDefine.KeyT:
                    return KeyT;
                case KeyBoardDefine.KeyU:
                    return KeyU;
                case KeyBoardDefine.KeyV:
                    return KeyV;
                case KeyBoardDefine.KeyW:
                    return KeyW;
                case KeyBoardDefine.KeyX:
                    return KeyX;
                case KeyBoardDefine.KeyY:
                    return KeyY;
                case KeyBoardDefine.KeyZ:
                    return KeyZ;
                case KeyBoardDefine.Key0:
                    return Key0;
                case KeyBoardDefine.Key1:
                    return Key1;
                case KeyBoardDefine.Key2:
                    return Key2;
                case KeyBoardDefine.Key3:
                    return Key3;
                case KeyBoardDefine.Key4:
                    return Key4;
                case KeyBoardDefine.Key5:
                    return Key5;
                case KeyBoardDefine.Key6:
                    return Key6;
                case KeyBoardDefine.Key7:
                    return Key7;
                case KeyBoardDefine.Key8:
                    return Key8;
                case KeyBoardDefine.Key9:
                    return Key9;
                case KeyBoardDefine.KeyNumpad0:
                    return KeyNumpad0;
                case KeyBoardDefine.KeyNumpad1:
                    return KeyNumpad1;
                case KeyBoardDefine.KeyNumpad2:
                    return KeyNumpad2;
                case KeyBoardDefine.KeyNumpad3:
                    return KeyNumpad3;
                case KeyBoardDefine.KeyNumpad4:
                    return KeyNumpad4;
                case KeyBoardDefine.KeyNumpad5:
                    return KeyNumpad5;
                case KeyBoardDefine.KeyNumpad6:
                    return KeyNumpad6;
                case KeyBoardDefine.KeyNumpad7:
                    return KeyNumpad7;
                case KeyBoardDefine.KeyNumpad8:
                    return KeyNumpad8;
                case KeyBoardDefine.KeyNumpad9:
                    return KeyNumpad9;
                case KeyBoardDefine.KeyMultiply:
                    return KeyMultiply;
                case KeyBoardDefine.KeyAdd:
                    return KeyAdd;
                case KeyBoardDefine.KeySeparator:
                    return KeySeparator;
                case KeyBoardDefine.KeySubtract:
                    return KeySubtract;
                case KeyBoardDefine.KeyDecimal:
                    return KeyDecimal;
                case KeyBoardDefine.KeyDivide:
                    return KeyDivide;
                case KeyBoardDefine.KeyF1:
                    return KeyF1;
                case KeyBoardDefine.KeyF2:
                    return KeyF2;
                case KeyBoardDefine.KeyF3:
                    return KeyF3;
                case KeyBoardDefine.KeyF4:
                    return KeyF4;
                case KeyBoardDefine.KeyF5:
                    return KeyF5;
                case KeyBoardDefine.KeyF6:
                    return KeyF6;
                case KeyBoardDefine.KeyF7:
                    return KeyF7;
                case KeyBoardDefine.KeyF8:
                    return KeyF8;
                case KeyBoardDefine.KeyF9:
                    return KeyF9;
                case KeyBoardDefine.KeyF10:
                    return KeyF10;
                case KeyBoardDefine.KeyF11:
                    return KeyF11;
                case KeyBoardDefine.KeyF12:
                    return KeyF12;
            }

            throw new ArgumentException($"{nameof(keyBoardDefine)} not define for {keyBoardDefine}");
        }
    }
}
