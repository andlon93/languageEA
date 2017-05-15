import matplotlib.pyplot as plt
import networkx as nx

file1 = "C:/Users/andrl/Desktop/masterStuff/MasterData/Graph1.txt"
file10 = "C:/Users/andrl/Desktop/masterStuff/MasterData/Graph10.txt"
file20 = "C:/Users/andrl/Desktop/masterStuff/MasterData/Graph20.txt"
file30 = "C:/Users/andrl/Desktop/masterStuff/MasterData/Graph30.txt"
file40 = "C:/Users/andrl/Desktop/masterStuff/MasterData/Graph40.txt"
file50 = "C:/Users/andrl/Desktop/masterStuff/MasterData/Graph50.txt"
file60 = "C:/Users/andrl/Desktop/masterStuff/MasterData/Graph60.txt"
file70 = "C:/Users/andrl/Desktop/masterStuff/MasterData/Graph70.txt"
file80 = "C:/Users/andrl/Desktop/masterStuff/MasterData/Graph80.txt"
file90 = "C:/Users/andrl/Desktop/masterStuff/MasterData/Graph90.txt"
file100 = "C:/Users/andrl/Desktop/masterStuff/MasterData/Graph100.txt"
def plotting(file, gen):
    opened  = open(file, 'r')
    d = {}
    for line in opened:
       t = (line.split(','))
       t.remove('\n')
       d[t[0]] = t[1: len(t)]



    G=nx.Graph()

    for i in d:
        for j in d[i]:
            G.add_edge(i, j)


    elarge=[(u,v) for (u,v,d) in G.edges(data=True)]

    pos=nx.spring_layout(G) # positions for all nodes

    # nodes
    nx.draw_networkx_nodes(G,pos,node_size=100)

    # edges
    nx.draw_networkx_edges(G,pos,edgelist=elarge,
                        width=1)

    # labels
    #nx.draw_networkx_labels(G,pos,font_size=20,font_family='sans-serif')

    plt.axis('off')
    plt.savefig("Data/graph"+gen+".png") # save as png
    plt.show() # display

plotting(file1, '1')
plotting(file10, '10')
plotting(file20, '20')
plotting(file30, '30')
plotting(file40, '40')
plotting(file50, '50')
plotting(file60, '60')
plotting(file70, '70')
plotting(file80, '80')
plotting(file90, '90')
plotting(file100, '100')
