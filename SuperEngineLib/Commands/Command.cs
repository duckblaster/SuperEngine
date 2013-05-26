using System;
namespace SuperEngine.Commands {
	public class Command {
		private ParameterGroup parameters;
		public ParameterGroup Parameters {
			get {
				return this.parameters;
			}
		}

        public virtual void Run()
        {
        }
		
		public Command() {
		}
	}
}

