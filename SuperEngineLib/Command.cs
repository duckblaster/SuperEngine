using System;
namespace FPRPG.Commands {
	public class Command {
		private ParameterGroup parameters;
		public ParameterGroup Parameters {
			get {
				return this.parameters;
			}
		}
		
		public abstract void Run() {
			
		}
		
		public Command() {
		}
	}
}

