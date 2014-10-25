namespace Core
{
    public class Register
    {
        public static readonly Register A = new Register(0);
        public static readonly Register B = new Register(1);
        public static readonly Register C = new Register(2);
        public static readonly Register D = new Register(3);
        public static readonly Register E = new Register(4);
        public static readonly Register H = new Register(5);
        public static readonly Register L = new Register(6);

        private Register(int index)
        {
            _index = index;
        }

        private readonly int _index;

        public static implicit operator int(Register register)
        {
            return register._index;
        }
    }
}