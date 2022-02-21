namespace Tatelier.Score.Component.NoteSystem
{
    public static class NoteTypeChar
    {
        public const char None = '0';
        public const char Don = '1';
        public const char Kat = '2';
        public const char DonBig = '3';
        public const char KatBig = '4';
        public const char Roll = '5';
        public const char RollBig = '6';
        public const char Balloon = '7';
        public const char End = '8';
        public const char Dull = '9';

        public static bool IsNoteType(char type)
        {
            return None <= type && type <= Dull;
        }

        public static NoteType GetNoteType(char type)
        {
            return (NoteType)type;
        }
    }

    public enum NoteType
    {
        None = NoteTypeChar.None,
        Don = NoteTypeChar.Don,
        Kat = NoteTypeChar.Kat,
        DonBig = NoteTypeChar.DonBig,
        KatBig = NoteTypeChar.KatBig,
        Roll = NoteTypeChar.Roll,
        RollBig = NoteTypeChar.RollBig,
        Balloon = NoteTypeChar.Balloon,
        End = NoteTypeChar.End,
        Dull = NoteTypeChar.Dull,
    }
}
