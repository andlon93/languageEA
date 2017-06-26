from matplotlib import pyplot as plt
from matplotlib import rc
import numpy as np
import copy

rc('font', **{'family': 'serif', 'serif': ['Computer Modern']})
rc('text', usetex=True)
path = "C:/Users/andrl/Desktop/Experiment 1/"
Fitnessfile = path+"FitnessData.txt"
Degreefile = path+"DegreeData.txt"
Dialoguefile = path+"DialogueData.txt"
Wordfile = path+"UniqueWordsData.txt"
LearnrateFile =  path+"LearnRate.txt"
MaxFitnessFile = path+"MaxFitness.txt"
avgVocLenFile = path+"avgVocLen.txt"
SpeakParentsGeneFile = path+"SpeakToParents.txt"
ExtrovertFile = path+"Extrovert.txt"
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
    plt.xlabel(r'Generations', fontsize=30)#r'$f(v)$', fontsize=30)
    plt.ylabel(r'Fitness', fontsize=30)#r'Speed $v$ [m/s]', fontsize=30)
    plt.xticks(np.arange(0,len(d1),10))
    #plt.yticks(np.arange(0,1, 0.1)    )
    plt.plot(range(len(d1)), d1)#, label=r'Average fitness')
    plt.tick_params(labelsize=20)
    #plt.plot(range(len(d2)), d2, label="Highest fitness")
    #plt.legend(loc=0)
    plt.show()
    # --- Saving figure.
    #plt.tight_layout()
    if(isSaveFig):
        plt.savefig("C:/Users/andrl/Master-thesis/fig/Results/Exp1/Fitness1", bbox_inches='tight')
def plot_words(name, d1):
    plt.ylabel(r'Unique words', fontsize=30)#r'Speed $v$ [m/s]', fontsize=30)
    plt.xlabel(r'Generations', fontsize=30)#r'$f(v)$', fontsize=30)
    plt.xticks(np.arange(0,len(d1),10))
    #plt.yticks(np.arange(0,1,0.1)    )
    plt.plot(range(len(d1)), d1)#, label= r'Nummber of unique highest ranked words')
    plt.tick_params(labelsize=20)
    #plt.legend(loc=0)
    plt.show()
    # --- Saving figure.
    #plt.tight_layout()
    if(isSaveFig):
        plt.savefig("C:/Users/andrl/Master-thesis/fig/Results/Exp1/UniqueWords1", bbox_inches='tight')
def plot_words2(name, d1, d2):
    #plt.ylabel("Unique highest ranked words", fontsize=30)#r'Speed $v$ [m/s]', fontsize=30)
    plt.xlabel(r'Generations', fontsize=30)#r'$f(v)$', fontsize=30)
    plt.xticks(np.arange(0,len(d1),10))
    #plt.yticks(np.arange(0,1,0.1)    )
    #plt.plot(range(100), d1, label="Nummber of unique highest ranked words")
    plt.plot(range(len(d1)), d1)#, label=r'Successful dialogues / All dialogues')
    plt.plot(range(len(d2)), d2)#, label=r'Average vocabulary length')
    plt.tick_params(labelsize=20)
    #plt.legend(loc=0)
    plt.show()
    # --- Saving figure.
    #plt.tight_layout()
    if(isSaveFig):
        plt.savefig("C:/Users/andrl/Master-thesis/fig/Results/Exp1/Vocabulary1", bbox_inches='tight')
def plot_genes(name, d1, d2, d3):
    #plt.ylabel("Genes")#r'Speed $v$ [m/s]', fontsize=30)
    plt.xlabel(r'Generations', fontsize=30)#r'$f(v)$', fontsize=30)
    plt.xticks(np.arange(0,len(d1),10))
    #plt.yticks(np.arange(0,1,0.1)    )
    #plt.plot(range(100), d1, label="Nummber of unique highest ranked words")
    plt.plot(range(len(d1)), d1)#, label=r'Learning rate')
    plt.plot(range(len(d2)), d2)#, label=r'Probability of speaking to parents')
    plt.plot(range(len(d3)), d3)#, label=r'Average extrovert probability')
    plt.tick_params(labelsize=20)
    #plt.legend(loc=0)
    plt.show()
    # --- Saving figure.
    #plt.tight_layout()
    if(isSaveFig):
        plt.savefig("C:/Users/andrl/Master-thesis/fig/Results/Exp1/Genes1", bbox_inches='tight')
def plot_degree(name, d1):
    plt.ylabel(r'Average degree', fontsize=30)#r'Speed $v$ [m/s]', fontsize=30
    plt.xlabel(r'Generations', fontsize=30)#r'$f(v)$', fontsize=30)
    plt.xticks(np.arange(0,len(d1),10))
    #plt.yticks(np.arange(0,1,0.1)    )
    #plt.plot(range(100), d1, label="Nummber of unique highest ranked words")
    plt.plot(range(len(d1)), d1)#, label=r'Average degree')
    plt.tick_params(labelsize=20)
    #plt.legend(loc=0)
    plt.show()
    # --- Saving figure.
    #plt.tight_layout()
    if(isSaveFig):
        plt.savefig("C:/Users/andrl/Master-thesis/fig/Results/Exp1/Degree1", bbox_inches='tight')
#
#
#path = ""
isSaveFig = False
plot_fitness("Fitness", FitnessData, MaxFitnessData)
plot_words("Words", WordsData)
plot_words2("Words2", DialogueData, avgVocLenData)
plot_degree("Degree", DegreeData)
plot_genes("Genes", LearnRateData, SpeakParentsGeneData, ExtrovertData)
#
