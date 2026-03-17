using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.Asistente.ArmadoDeLeyenda;
using ZooLogicSA.Promociones.Informantes;

namespace ZooLogicSA.Promociones.Asistente
{
    public class ObservadorPromocionSingleThread : IObservadorServicioPromociones
    {
        private IArmadorDeLeyenda armador;
        private List<string> errores;
        private List<PromocionesEstado> infoPromociones;

        public List<PromocionesEstado> ObtenerResultados(bool soloAutomaticas = false, bool soloConBeneficio = false)
        {
            if (this.errores.Count > 0)
            {
                throw new Exception(String.Join("\r\n", this.errores.ToArray()));
            }


            List<PromocionesEstado> retorno = new List<PromocionesEstado>();
            retorno = this.infoPromociones.OrderByDescending(x => x.Destacada).ThenByDescending(x => x.Beneficio).ThenBy(x => x.Id).ToList();

            if (soloAutomaticas)
            {
                retorno = retorno.Where(x => x.Automatica).ToList();
            }

            if (soloConBeneficio)
            {
                retorno = retorno.Where(x => x.Beneficio > 0).ToList();
            }

            return retorno;
        }

        public ObservadorPromocionSingleThread()
        {
            FactoriaArmadorDeLeyenda fac = new FactoriaArmadorDeLeyenda();

            this.armador = fac.ObtenerArmadorDeLeyenda();

            this.errores = new List<string>();

            this.infoPromociones = new List<PromocionesEstado>();
        }

        public void InformarApagado()
        {
        }

        public void InformarDebug(string mensaje)
        {
        }

        public void PresentarPromocionesAplicables(List<InformacionPromocion> informacionPromociones)
        {
            try
            {
                foreach (InformacionPromocion promos in informacionPromociones)
                {

                    if (promos.Promocion == null)
                    {
                        continue;
                    }

                    if (promos.Promocion.Visualizacion == FormatoPromociones.VisualizacionPromocionAsistenteType.NoVisible)
                    {
                    }
                    else
                    {
                        string leyendaFaltanteCompleta;

                        if (promos.Afectaciones == 0)
                        {
                            leyendaFaltanteCompleta = this.armador.Armar(promos.infoIncumplida);
                        }
                        else
                        {
                            leyendaFaltanteCompleta = null;
                        }

                        PromocionesEstado p = new PromocionesEstado()
                        {
                            Id = promos.IdPromocion.TrimEnd(),
                            Descripcion = promos.Promocion.Descripcion,
                            LeyendaCliente = promos.Promocion.LeyendaAsistente,
                            FaltanteCompleto = leyendaFaltanteCompleta,
                            Faltante = leyendaFaltanteCompleta,
                            Beneficio = promos.MontoBeneficio,
                            Estado = (promos.Afectaciones > 0) ? estado.Cumplida : this.ObtenerEstado(promos.infoIncumplida),
                            Destacada = (promos.Promocion.Visualizacion == FormatoPromociones.VisualizacionPromocionAsistenteType.Destacada),
                            Automatica = promos.Promocion.AplicaAutomaticamente
                        };

                        this.infoPromociones.Add(p);
                    }
                }
            }
            catch (Exception ex)
            {
                this.errores.Add(ex.ToString());
            }
        }

        private estado ObtenerEstado(InformacionPromocionIncumplida info)
        {
            if (info != null)
            {
                if (info.Resultados != null)
                {
                    return (info.Resultados.Exists(x => x.Satisfecho > 0 && !x.PartPromo.Codigo.Equals("COMPROBANTE"))) ? estado.Parcial : estado.Incumplida;
                }
            }

            return estado.Incumplida;
        }

        public void ProcesarError(Exception ex)
        {
            this.errores.Add(ex.ToString());
        }

        public void ProcesarErrorEnPromocion(InformacionPromocionIncumplida idPromocion, Exception ex)
        {
            this.errores.Add(ex.ToString());
        }
    }
}
