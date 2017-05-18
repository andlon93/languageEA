from matplotlib import pyplot as plt
from matplotlib import rc
import numpy as np
import copy
#rc('font', **{'family': 'serif', 'serif': ['Computer Modern']})
#rc('text', usetex=True)

Fitnessfile = "C:/Users/andrl/Desktop/masterStuff/MasterData/FitnessData.txt"
Degreefile = "C:/Users/andrl/Desktop/masterStuff/MasterData/DegreeData.txt"
Dialoguefile = "C:/Users/andrl/Desktop/masterStuff/MasterData/DialogueData.txt"
Wordfile = "C:/Users/andrl/Desktop/masterStuff/MasterData/UniqueWordsData.txt"
LearnrateFile =  "C:/Users/andrl/Desktop/masterStuff/MasterData/LearnRate.txt"
MaxFitnessFile = "C:/Users/andrl/Desktop/masterStuff/MasterData/MaxFitness.txt"
avgVocLenFile = "C:/Users/andrl/Desktop/masterStuff/MasterData/avgVocLen.txt"
SpeakParentsGeneFile = "C:/Users/andrl/Desktop/masterStuff/MasterData/SpeakToParents.txt"
Fitness = open(Fitnessfile, 'r')
Degree = open(Degreefile, 'r')
Dialogue = open(Dialoguefile, 'r')
Words = open(Wordfile, 'r')
LearnRate = open(LearnrateFile, 'r')
MaxFitness = open(MaxFitnessFile, 'r')
avgVocLen = open(avgVocLenFile, 'r')
SpeakParentsGene= open(SpeakParentsGeneFile, 'r')
FitnessData = []
DegreeData = []
DialogueData = []
WordsData = []
LearnRateData = []
MaxFitnessData = []
avgVocLenData = []
SpeakParentsGeneData = []
for line in Fitness:
    FitnessData.append(float(line))
for line in Degree:
    DegreeData.append(float(line))
for line in Dialogue:
    DialogueData.append(float(line))
for line in Words:
    WordsData.append(int(line))
for line in LearnRate:
    LearnRateData.append(float(line))
for line in MaxFitness:
    MaxFitnessData.append(float(line))
for line in avgVocLen:
    avgVocLenData.append(float(line))
for line in SpeakParentsGene:
    SpeakParentsGeneData.append(float(line))
# --- Function: Plot the distribution of speeds in a histogram.
# input : list of speeds, filename of savefile.
# output: none, but saves a figure of the histogram.
def plot_data(data, x, y, name, filename):
    # --- Plot histogram.
    #histogram = plt.figure()
    #plt.bar(FitnessData, hist, width=100, color='blue')

    plt.xlabel(x)#r'Speed $v$ [m/s]', fontsize=30)
    plt.ylabel(y)#r'$f(v)$', fontsize=30)
    plt.xticks(np.arange(0,len(data),10))
    plt.yticks(np.linspace(0, max(data), 10)    )
    plt.plot(range(len(data)), data, label=name)
    plt.show()
    # --- Saving figure.
    #plt.tight_layout()
    plt.savefig("Data/"+name)
plot_data(FitnessData, "Number of generations", "Fitness", "Fitness", "tull")
plot_data(WordsData, "Number of generations", "Number of unique highest ranked words", "unique words", "tull")
plot_data(DegreeData, "Number of generations", "Average degree", "Average degree per agent", "tull")
plot_data(DialogueData, "Number of generations", "Succesfull dialogues", "succesfull dialogues over total dialogues", 'tull')
plot_data(LearnRateData, "Number of generations", "learn rate", "Average learn rate", "tull")
plot_data(MaxFitnessData, "Number of generations", "Max fitness", "Fitness", "tull")
plot_data(avgVocLenData, "Number of generations", "average vocabulary length", "vocabulary length", "tull")
plot_data(SpeakParentsGeneData, "Number of generations", "Speak to parents gene", "Speak to parents gene", "tull")
