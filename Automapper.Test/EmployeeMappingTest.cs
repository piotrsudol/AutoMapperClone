using AutoMapperClone;

namespace Automapper.Test
{
    public class EmployeeMappingTest
    {

        private Employee basic = new Employee()
        {
            FirstName = "Piotr",
            LastName = "Olender"
        };

        [Fact]
        public void ExpressionMapperSimpleTest()
        {
            var from = basic;

            var to = ExpressionMapper.To<EmployeeDto>(from);

            CompareAllFields(from, to);
        }

        [Fact]
        public void ReflectionMapperSimpleTest() {
            var from = basic;

            var to = ExpressionMapper.To<EmployeeDto>(from);

            CompareAllFields(from, to);
        }

        private void CompareAllFields(Employee from, EmployeeDto to) {
            Assert.Equal(from.FirstName, to.FirstName);
            Assert.Equal(from.LastName, to.LastName);
        }
    }
}