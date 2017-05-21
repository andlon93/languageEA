from matplotlib import pyplot as plt
from matplotlib import rc
import numpy as np
import copy
#rc('font', **{'family': 'serif', 'serif': ['Computer Modern']})
#rc('text', usetex=True)

Fitnessfile = "C:/Users/andrl/Desktop/masterStuff/MasterData/Figures/Experiment 2/FitnessData.txt"
Degreefile = "C:/Users/andrl/Desktop/masterStuff/MasterData/Figures/Experiment 2/DegreeData.txt"
Dialoguefile = "C:/Users/andrl/Desktop/masterStuff/MasterData/Figures/Experiment 2/DialogueData.txt"
Wordfile = "C:/Users/andrl/Desktop/masterStuff/MasterData/Figures/Experiment 2/UniqueWordsData.txt"
LearnrateFile =  "C:/Users/andrl/Desktop/masterStuff/MasterData/Figures/Experiment 2/LearnRate.txt"
MaxFitnessFile = "C:/Users/andrl/Desktop/masterStuff/MasterData/Figures/Experiment 2/MaxFitness.txt"
avgVocLenFile = "C:/Users/andrl/Desktop/masterStuff/MasterData/Figures/Experiment 2/avgVocLen.txt"
SpeakParentsGeneFile = "C:/Users/andrl/Desktop/masterStuff/MasterData/Figures/Experiment 2/SpeakToParents.txt"
ExtrovertFile = "C:/Users/andrl/Desktop/masterStuff/MasterData/Figures/Experiment 2/Extrovert.txt"
Fitness = open(Fitnessfile, 'r')
Degree = open(Degreefile, 'r')
Dialogue = open(Dialoguefile, 'r')
Words = open(Wordfile, 'r')
LearnRate = open(LearnrateFile, 'r')
MaxFitness = open(MaxFitnessFile, 'r')
avgVocLen = open(avgVocLenFile, 'r')
Extrovert = open(ExtrovertFile, 'r')
SpeakParentsGene= open(SpeakParentsGeneFile, 'r')
FitnessData = []
DegreeData = []
DialogueData = []
WordsData = []
LearnRateData = []
MaxFitnessData = []
avgVocLenData = []
ExtrovertData = []
SpeakParentsGeneData = []
for line in Extrovert:
    ExtrovertData.append(float(line))
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
def plot_fitness(name, d1, d2):
    plt.xlabel("Generations", fontsize=20)#r'$f(v)$', fontsize=30)
    plt.ylabel("Fitness", fontsize=20)#r'Speed $v$ [m/s]', fontsize=30)
    plt.xticks(np.arange(0,100,10))
    #plt.yticks(np.arange(0,1, 0.1)    )
    plt.plot(range(100), d1, label="Average fitness")
    plt.plot(range(100), d2, label="Highest fitness")
    plt.legend(loc=4)
    plt.show()
    # --- Saving figure.
    #plt.tight_layout()
    if(isSaveFig):
        plt.savefig(path+name)
def plot_words(name, d1):
    plt.ylabel("Unique words", fontsize=20)#r'Speed $v$ [m/s]', fontsize=30)
    plt.xlabel("Generations", fontsize=20)#r'$f(v)$', fontsize=30)
    plt.xticks(np.arange(0,100,10))
    #plt.yticks(np.arange(0,1,0.1)    )
    plt.plot(range(100), d1, label="Nummber of unique highest ranked words")
    plt.show()
    # --- Saving figure.
    #plt.tight_layout()
    if(isSaveFig):
        plt.savefig(path+name)
def plot_words2(name, d1, d2):
    #plt.ylabel("Unique highest ranked words", fontsize=30)#r'Speed $v$ [m/s]', fontsize=30)
    plt.xlabel("Generations", fontsize=20)#r'$f(v)$', fontsize=30)
    plt.xticks(np.arange(0,100,10))
    #plt.yticks(np.arange(0,1,0.1)    )
    #plt.plot(range(100), d1, label="Nummber of unique highest ranked words")
    plt.plot(range(100), d1, label="Percentage of successful dialogues")
    plt.plot(range(100), d2, label="Average vocabulary length")
    plt.legend()
    plt.show()
    # --- Saving figure.
    #plt.tight_layout()
    if(isSaveFig):
        plt.savefig(path+name)
def plot_genes(name, d1, d2, d3):
    #plt.ylabel("Genes")#r'Speed $v$ [m/s]', fontsize=30)
    plt.xlabel("Generations", fontsize=20)#r'$f(v)$', fontsize=30)
    plt.xticks(np.arange(0,100,10))
    #plt.yticks(np.arange(0,1,0.1)    )
    #plt.plot(range(100), d1, label="Nummber of unique highest ranked words")
    plt.plot(range(100), d1, label="Learning rate")
    plt.plot(range(100), d2, label="Probability of speaking to parents")
    plt.plot(range(100), d3, label="Average extrovert probability")
    plt.legend(loc=4)
    plt.show()
    # --- Saving figure.
    #plt.tight_layout()
    if(isSaveFig):
        plt.savefig(path+name)
def plot_degree(name, d1):
    plt.ylabel("Average degree", fontsize=20)#r'Speed $v$ [m/s]', fontsize=30)
    plt.xlabel("Generations", fontsize=20)#r'$f(v)$', fontsize=30)
    plt.xticks(np.arange(0,100,10))
    #plt.yticks(np.arange(0,1,0.1)    )
    #plt.plot(range(100), d1, label="Nummber of unique highest ranked words")
    plt.plot(range(100), d1, label="Average degree")

    plt.show()
    # --- Saving figure.
    #plt.tight_layout()
    if(isSaveFig):
        plt.savefig(path+name)
#
#
path = "C:/Users/andrl/Desktop/masterStuff/MasterData/Figures/Experiment 2"
isSaveFig = False
plot_fitness("Fitness", FitnessData, MaxFitnessData)
plot_words("Words", WordsData)
plot_words2("Words2", DialogueData, avgVocLenData)
plot_degree("Degree", DegreeData)
plot_genes("Genes", LearnRateData, SpeakParentsGeneData, ExtrovertData)
#
