# TasksWorkersSimulator
Simulator for evaluate the best strategy for grading workers for the assigned tasks.

## Usage:
To start using the simulator you need to supply execution details (Distributions, iterations, initial number of workers etc.) </br>
Create xml file and name it "SimExe.xml", put it in the same location as the executable file (TaskSimulationCmd.exe). </br>
(There is an example file with the bineries you downloaded) 
### File structure 
In the file you can deffine the parameters for each iteration execution: </br>
The <b>Execution</b> element opens an execution instructions.  </br>
It has folowing attributes:  </br>
- InitialNumOfWorkers - The number of workers in the system at time 0 of the simulation
- MaxSimulationTime - The maximal time for the simulation clock
The folowing elements are the "Task arrival", "Worker arrival\leave" rates.
attributes are:
- Type: The type of the distribution
- Params: The distribution params (each type has its own params format) for example:
  - ContinuousUniform format:"mean std"
  - Exponential format: "rate"
- All the continius distibutions from [MathNet](https://numerics.mathdotnet.com/api/MathNet.Numerics.Distributions/) are supported (Laplace, Normal, StudentT etc.) 

