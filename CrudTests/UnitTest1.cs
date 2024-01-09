namespace CrudTests
{
    public class UnitTest1
    {
        //here unit test should be first and then functionality.

        [Fact]
        public void Test1()
        {
            //steps for every unit test
            //arrange : declaration of variable and collecting inputs

            MyMath mm = new MyMath();
            int input1 = 10, input2 = 5;
            int expected = 15;

            //act  :which method you want to test
            int actual = mm.Add(input1, input2);

            //assert : comparing actual value and collected value
            Assert.Equal(expected, actual);
        }
    }
}