using System;
using System.Collections.Generic;
namespace DeepSpace
{
	class Estrategia
	{
		//CONSULTA 1
		//Se retorna la profundidad del arbol
		public String Consulta1(ArbolGeneral<Planeta> arbol)
		{
			//Utilizando una cola se recorre el arbol
			Cola<ArbolGeneral<Planeta>> cola = new Cola<ArbolGeneral<Planeta>>();
			ArbolGeneral<Planeta> arbolTemp;
			int nivel_actual = -1;
			cola.encolar(arbol);
			//Con el null se indica un cambio de nivel
			cola.encolar(null);

			while (!cola.esVacia())
			{
				arbolTemp = cola.desencolar();
				//Se vetifica si se ha realizado un cambio de nivel
				if (arbolTemp == null)
				{
					nivel_actual++;
					if (!cola.esVacia())
						cola.encolar(null);
					continue;
				}

				foreach (ArbolGeneral<Planeta> arbolHijo in arbolTemp.getHijos())
				{
					cola.encolar(arbolHijo);
				}
			}
			return "Profundidad del arbol: " + nivel_actual;
		}


		//CONSULTA 2
		//Se retorna un texto con la cantidad de planetas ubicados en las hojas que tengan poblacion mayor a 3
		public String Consulta2(ArbolGeneral<Planeta> arbol)
		{
			//Utilizando una cola se recorre el arbol correspondiente
			Cola<ArbolGeneral<Planeta>> cola = new Cola<ArbolGeneral<Planeta>>();
			ArbolGeneral<Planeta> arbolTemp;
			cola.encolar(arbol);
			//NULL indica un cambio de nivel
			cola.encolar(null);
			int cant = 0;
			while (!cola.esVacia())
			{
				arbolTemp = cola.desencolar();
				//Se vetifica si se ha realizado un cambio de nivel
				if (arbolTemp == null)
				{
					if (!cola.esVacia())
						cola.encolar(null);
					continue;
				}

				if (arbolTemp.esHoja())
				{
					if (arbolTemp.getDatoRaiz().Poblacion() > 3)
					{
						cant++;
					}
					continue;
				}
				//NO es hoja, por lo que se encolan sus hijos
				foreach (ArbolGeneral<Planeta> hijo in arbolTemp.getHijos())
				{
					cola.encolar(hijo);
				}
			}
			return "Hojas con poblacion mayor a 3: " + cant;
		}


		//CONUSLTA 3
		//Se calcula y retorna en formato de texto el numero de planeta por cada nivel que posee una
		//poblacion mayor a la promedio del arbol
		public String Consulta3(ArbolGeneral<Planeta> arbol)
		{
			//Se recorre el arbol utlizando una cola y se calcula la poblacion promedio del mismo
			Cola<ArbolGeneral<Planeta>> cola = new Cola<ArbolGeneral<Planeta>>();
			ArbolGeneral<Planeta> arbolTemp;
			int cantPlanetas = 0;
			int poblacionTotal = 0;
			int promedio;
			cola.encolar(arbol);
			//El null indica un cambio de nivel
			cola.encolar(null);
			while (!cola.esVacia())
			{
				arbolTemp = cola.desencolar();
				//Verifica si se ha realizado un cambio de nivel
				if (arbolTemp == null)
				{
					if (!cola.esVacia())
						cola.encolar(null);
					continue;
				}
				cantPlanetas++;
				poblacionTotal = poblacionTotal + arbolTemp.getDatoRaiz().Poblacion();

				if (!arbolTemp.esHoja())
				{
					foreach (ArbolGeneral<Planeta> hijo in arbolTemp.getHijos())
						cola.encolar(hijo);
				}
			}
			promedio = poblacionTotal / cantPlanetas;
			cantPlanetas = 0;

			//Se recorre nuevamente el arbol para obtener por cada nivel aquellos planetas que poseen una poblacion mayor a la promedio
			int cantNiveles = 1;
			cantPlanetas = 0;
			string resultado = "";
			cola.encolar(arbol);
			cola.encolar(null);
			while (!cola.esVacia())
			{
				arbolTemp = cola.desencolar();
				//Verifica si se ha realizado un cambio de nivel
				if (arbolTemp == null)
				{
					resultado = resultado + "\nNivel: " + cantNiveles + " --- " + cantPlanetas;
					cantNiveles++;
					if (!cola.esVacia())
						cola.encolar(null);
					//Se vuelve a 0 la variable ya que se comienza un nuevo nivel
					cantPlanetas = 0;
					continue;
				}
				if (arbolTemp.getDatoRaiz().Poblacion() > promedio)
				{
					cantPlanetas++;
				}
				if (!arbolTemp.esHoja())
				{
					foreach (ArbolGeneral<Planeta> hijo in arbolTemp.getHijos())
						cola.encolar(hijo);
				}
			}
			return "\nPlanetas por nivel con poblacion mayor a promedio (" + promedio + ")" + resultado;
		}

		//CALULAR MOVIMIENTO
		//Los movimientos buscaran conquistar todos aquellos planetas descendientes del BOT
		private Movimiento movimiento;
		private int i = 0;
		private int j = 0;
		private int cant_hijos = 0;
		private List<ArbolGeneral<Planeta>> lista = new List<ArbolGeneral<Planeta>>();
		public Movimiento CalcularMovimiento(ArbolGeneral<Planeta> arbol)
		{
			//Uitilizando una cola se recorre el arbol correspondiente
			Cola<ArbolGeneral<Planeta>> cola = new Cola<ArbolGeneral<Planeta>>();
			ArbolGeneral<Planeta> arbolTemp;
			cola.encolar(arbol);

			while (!cola.esVacia())
			{
				arbolTemp = cola.desencolar();

				//Verificamos si el planeta es de la IA. Si lo es debemos conquitar sus hijos
				if (arbolTemp.getDatoRaiz().EsPlanetaDeLaIA())
				{
					//Obtenemos un listado de sus hijos
					lista = HijosBot(arbolTemp, new List<ArbolGeneral<Planeta>>());
					cant_hijos = lista.Count - 1;

					//Recorremos los hijos
					for (i = 0; i < cant_hijos; i++)
					{
						//Vetificamos si el siguiente hijo NO es planeta de la IA
						if (!lista[i + 1].getDatoRaiz().EsPlanetaDeLaIA())
						{
							//Vetificamos si es hoja
							if (lista[i + 1].esHoja())
							{
								//lanzamos tropas desde el planeta actual hacia dicho hijo para conquistarlo
								movimiento = new Movimiento(lista[i].getDatoRaiz(), lista[i + 1].getDatoRaiz());
								return movimiento;
							}
							//Si NO es una hoja lanzamos tropas desde el origen para conquistarlo
							movimiento = new Movimiento(lista[0].getDatoRaiz(), lista[i + 1].getDatoRaiz());
							return movimiento;
						}
					}
				}
				else
				{
					if (arbolTemp.getHijos().Count != 0)
						foreach (ArbolGeneral<Planeta> hijo in arbolTemp.getHijos())
							cola.encolar(hijo);
				}
			}
			i = 0;

            //Ahora reagrupamos las tropas (se envian tropas desde los planetas conquitados al del BOT)
            for (i = cant_hijos - j; i > 0; i--)
			{
				j++;
				//Verificamos si el nodo actual es hoja
				if (lista[i].esHoja())
				{
					//Realizamos un movimiento de tropas desde dicho nodo hacia su padre
					movimiento = new Movimiento(lista[i].getDatoRaiz(), lista[i - 1].getDatoRaiz());
					return movimiento;
				}
				//Si NO es hoja realizamos un movimiento hacia el nodo raiz
				movimiento = new Movimiento(lista[i].getDatoRaiz(), lista[0].getDatoRaiz());
				return movimiento;
			}
			return null;
		}




		//De manera recursiva se retona una lista que contiene todos los hijos del BOT
		private List<ArbolGeneral<Planeta>> HijosBot(ArbolGeneral<Planeta> arbol, List<ArbolGeneral<Planeta>> lista)
		{
			List<ArbolGeneral<Planeta>> listaTemp = null;
			lista.Add(arbol);

			if (arbol.esHoja())
			{
				return lista;
			}

			foreach (ArbolGeneral<Planeta> arbolHijo in arbol.getHijos())
				listaTemp = HijosBot(arbolHijo, lista);

			return listaTemp;
		}
	}





}
