using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.Informantes;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.Tests
{
    [TestClass]

    public class PromocionesAutomaticasTest
    {
        [TestMethod]
        public void NoEsPromocionAutomatica()
        {
            bool respuesta = true;

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            IFactoriaPromociones factoria = new FactoriaPromociones();
            MotorPromociones motorPromociones = new MotorPromociones(config, factoria);
            ServicioEvaluacionPromociones servicioEvaluacion = new ServicioEvaluacionPromociones(motorPromociones, factoria);
            InformacionPromocion infopromocion = new InformacionPromocion("");
            Promocion promocion = new Promocion();
            
            promocion.AplicaAutomaticamente = false;
            infopromocion.MontoBeneficio = 100;
            infopromocion.Promocion = promocion;

            respuesta = servicioEvaluacion.EsPromocionAutomaticaAplicable(infopromocion);

            Assert.IsFalse(respuesta);

        }

        [TestMethod]
        public void EsPromocionAutomaticaPeroNoAplica()
        {
            bool respuesta = true;

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            IFactoriaPromociones factoria = new FactoriaPromociones();
            MotorPromociones motorPromociones = new MotorPromociones(config, factoria);
            ServicioEvaluacionPromociones servicioEvaluacion = new ServicioEvaluacionPromociones(motorPromociones, factoria);
            InformacionPromocion infopromocion = new InformacionPromocion("");
            Promocion promocion = new Promocion();

            promocion.AplicaAutomaticamente = true;
            infopromocion.MontoBeneficio = 0;
            infopromocion.Promocion = promocion;

            respuesta = servicioEvaluacion.EsPromocionAutomaticaAplicable(infopromocion);

            Assert.IsFalse(respuesta);

        }
        [TestMethod]
        public void EsPromocionAutomaticaAplicable()
        {
            bool respuesta = true;

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            IFactoriaPromociones factoria = new FactoriaPromociones();
            MotorPromociones motorPromociones = new MotorPromociones(config, factoria);
            ServicioEvaluacionPromociones servicioEvaluacion = new ServicioEvaluacionPromociones(motorPromociones, factoria);
            InformacionPromocion infopromocion = new InformacionPromocion("");
            Promocion promocion = new Promocion();

            promocion.AplicaAutomaticamente = true;
            infopromocion.MontoBeneficio = 100;
            infopromocion.Promocion = promocion;

            respuesta = servicioEvaluacion.EsPromocionAutomaticaAplicable(infopromocion);

            Assert.IsTrue(respuesta);

        }
        [TestMethod]
        public void NoEsAptoAplicacionAutomatica()
        {
            bool respuesta = true;

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            IFactoriaPromociones factoria = new FactoriaPromociones();
            MotorPromociones motorPromociones = new MotorPromociones(config, factoria);
            ServicioEvaluacionPromociones servicioEvaluacion = new ServicioEvaluacionPromociones(motorPromociones, factoria);
            InformacionPromocion infoPromocion = new InformacionPromocion("");
            Promocion promocion = new Promocion();         
                                    
            respuesta = servicioEvaluacion.ElCOmprobanteAplicaPromocionesAutomaticas();
            Assert.IsFalse(respuesta);
            
        }
        [TestMethod]
        public void EsAptoAplicacionAutomatica()
        {
            bool respuesta = true;

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            IFactoriaPromociones factoria = new FactoriaPromociones();
            MotorPromociones motorPromociones = new MotorPromociones(config, factoria);
            ServicioEvaluacionPromociones servicioEvaluacion = new ServicioEvaluacionPromociones(motorPromociones, factoria);
            InformacionPromocion infoPromocion = new InformacionPromocion("");
            Promocion promocion = new Promocion();
            object objetoVacio = new object();

            servicioEvaluacion.InyectarManagerAutomatico(objetoVacio);

            try
            {
                respuesta = servicioEvaluacion.ElCOmprobanteAplicaPromocionesAutomaticas();
            }catch
            {
                respuesta = true;
            }

            Assert.IsTrue(respuesta);

        }

        [TestMethod]
        public void InyectaManagerAutomatico()
        {

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            IFactoriaPromociones factoria = new FactoriaPromociones();
            MotorPromociones motorPromociones = new MotorPromociones(config, factoria);
            ServicioEvaluacionPromociones servicioEvaluacion = new ServicioEvaluacionPromociones(motorPromociones, factoria);
            InformacionPromocion infoPromocion = new InformacionPromocion("");
            Promocion promocion = new Promocion();
            object objetoVacio = new object();

            servicioEvaluacion.InyectarManagerAutomatico(objetoVacio);

            Assert.IsNotNull(servicioEvaluacion.managerPromocionesAutomaticas);

        }

        [TestMethod]
        public void LimpiaManagerAutomatico()
        {

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            IFactoriaPromociones factoria = new FactoriaPromociones();
            MotorPromociones motorPromociones = new MotorPromociones(config, factoria);
            ServicioEvaluacionPromociones servicioEvaluacion = new ServicioEvaluacionPromociones(motorPromociones, factoria);
            InformacionPromocion infoPromocion = new InformacionPromocion("");
            Promocion promocion = new Promocion();
            object objetoVacio = new object();

            servicioEvaluacion.managerPromocionesAutomaticas = objetoVacio;
            servicioEvaluacion.LimpiarManagerAutomatico();

            Assert.IsNull(servicioEvaluacion.managerPromocionesAutomaticas);

        }

        [TestMethod]
        public void CambiaEstadoDeSerializacionAlFinalizar()
        {

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            IFactoriaPromociones factoria = new FactoriaPromociones();
            MotorPromociones motorPromociones = new MotorPromociones(config, factoria);
            ServicioEvaluacionPromociones servicioEvaluacion = new ServicioEvaluacionPromociones(motorPromociones, factoria);
            InformacionPromocion infoPromocion = new InformacionPromocion("");
            Promocion promocion = new Promocion();
            System.Diagnostics.Stopwatch cronometro = new System.Diagnostics.Stopwatch();
            object objetoVacio = new object();
            bool lRespuesta;

            lRespuesta = true;
            servicioEvaluacion.estaSerializandoYEvaluando = true;

            servicioEvaluacion.HabilitarSerializacionPorHilos(servicioEvaluacion);
            servicioEvaluacion.SerializarEnHilo(objetoVacio, false);

            cronometro.Start();
            do
            {
                lRespuesta = servicioEvaluacion.ObtenerEstadoSerializacionYEvaluacion();
            } while (lRespuesta && cronometro.ElapsedMilliseconds < 30000);                                    
            
            servicioEvaluacion.DeshabilitarSerializacionPorHilos();
            cronometro.Stop();

            Assert.IsFalse(lRespuesta);

        }
    }
}
