using ZooLogicSA.Promociones;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace ZooLogicSA.Promociones.Tests
{
    [TestClass()]
    public class EvaluadorTest
    {
        [TestMethod()]
        public void EvaluarTest()
        {
            EvaluadorExpresion target = new EvaluadorExpresion();
            Assert.AreEqual( true, target.Evaluar( "true" ) );
            Assert.AreEqual( false, target.Evaluar( "false" ) );
            Assert.AreEqual( true, target.Evaluar( "true or false" ) );
            Assert.AreEqual( false, target.Evaluar( "true AND false" ) );
            Assert.AreEqual( false, target.Evaluar( "true && false" ) );
            Assert.AreEqual( true, target.Evaluar( "(false && false) || true" ) );
            Assert.AreEqual( true, target.Evaluar( "false && false || true" ) );
            Assert.AreEqual( false, target.Evaluar( "false && (false || true)" ) );
            Assert.AreEqual( false, target.Evaluar( "(false && (false || true))" ) );
            Assert.AreEqual( false, target.Evaluar( "Not (true)" ) );
            Assert.AreEqual( true, target.Evaluar( "Not (false)" ) );
        }
    }
}
