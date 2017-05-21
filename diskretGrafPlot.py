import matplotlib.pyplot as plt
import networkx as nx

file1 = "C:/Users/andrl/Desktop/masterStuff/MasterData/Figures/Experiment 2/Graph1.txt"
file5 = "C:/Users/andrl/Desktop/masterStuff/MasterData/Figures/Experiment 2/Graph5.txt"
file6 = "C:/Users/andrl/Desktop/masterStuff/MasterData/Figures/Experiment 2/Graph6.txt"
file10 = "C:/Users/andrl/Desktop/masterStuff/MasterData/Figures/Experiment 2/Graph10.txt"
file20 = "C:/Users/andrl/Desktop/masterStuff/MasterData/Figures/Experiment 2/Graph20.txt"
file30 = "C:/Users/andrl/Desktop/masterStuff/MasterData/Figures/Experiment 2/Graph30.txt"
file40 = "C:/Users/andrl/Desktop/masterStuff/MasterData/Figures/Experiment 2/Graph40.txt"
file50 = "C:/Users/andrl/Desktop/masterStuff/MasterData/Figures/Experiment 2/Graph50.txt"
file60 = "C:/Users/andrl/Desktop/masterStuff/MasterData/Figures/Experiment 2/Graph60.txt"
file70 = "C:/Users/andrl/Desktop/masterStuff/MasterData/Figures/Experiment 2/Graph70.txt"
file80 = "C:/Users/andrl/Desktop/masterStuff/MasterData/Figures/Experiment 2/Graph80.txt"
file90 = "C:/Users/andrl/Desktop/masterStuff/MasterData/Figures/Experiment 2/Graph90.txt"
file100 = "C:/Users/andrl/Desktop/masterStuff/MasterData/Figures/Experiment 2/Graph100.txt"
def plotting(file, gen):
    print("Generasjon: ", gen)
    opened  = open(file, 'r')
    d = {}
    for line in opened:
       t = (line.split(','))
       if(t.__contains__('\n')):
            t.remove('\n')
       d[t[0]] = t[1: len(t)]



    G=nx.Graph()

    for i in d:
        for j in d[i]:
            G.add_edge(i, j)


    elarge=[(u,v) for (u,v,d) in G.edges(data=True)]

    pos=nx.spring_layout(G) # positions for all nodes

    # nodes
    nx.draw_networkx_nodes(G,pos,node_size=10)

    # edges
    nx.draw_networkx_edges(G,pos,edgelist=elarge,
                        width=0.2)

    # labels
    #nx.draw_networkx_labels(G,pos,font_size=20,font_family='sans-serif')

    plt.axis('off')
    plt.savefig("C:/Users/andrl/Desktop/masterStuff/MasterData/Figures/Experiment 2/_graph"+gen+".png") # save as png
    plt.show() # display

plotting(file1, '1')
plotting(file5, '5')
#plotting(file6, '6')
plotting(file10, '10')
plotting(file20, '20')
plotting(file40, '40')
#plotting(file50, '50')
plotting(file60, '60')
#plotting(file70, '70')
plotting(file80, '80')
#plotting(file90, '90')
plotting(file100, '100')
