#!/usr/bin/env python
# coding: utf-8

# In[3]:


import pandas as pd
from sklearn.model_selection import train_test_split
from sklearn.linear_model import LogisticRegression
from sklearn.neighbors import KNeighborsClassifier
from sklearn.tree import DecisionTreeClassifier
from sklearn.ensemble import RandomForestClassifier
from sklearn.metrics import accuracy_score
import numpy as np


# In[4]:


X = pd.read_csv('ECG_dataX.csv')
Y = pd.read_csv('ECG_dataY.csv')


# In[5]:


X=X.values
X.shape 


# In[6]:


Y=Y.values
Y.shape


# In[7]:


Y=Y.reshape(-1)
Y.shape

# Splitting the data
X_train, X_test, Y_train, Y_test = train_test_split(X, Y, test_size=0.2, random_state=0)
# Verifying the shapes of the splits
X_train.shape, Y_train.shape, X_test.shape, Y_test.shape


# In[8]:


knn = KNeighborsClassifier()
log_reg = LogisticRegression(solver='newton-cg')
dt = DecisionTreeClassifier(random_state=0)
rf = RandomForestClassifier(random_state=0)


# In[9]:


# List of classifiers for ease of iteration
classifiers = [('KNeighbors', knn),
               ('LogisticRegression', log_reg),
               ('DecisionTree', dt),
               ('RandomForest', rf)]

# Prepare a dictionary to store accuracies
accuracies = {
    ' ': [],
    'Training Accuracy': [],
    'Test Accuracy': []
}


# In[10]:


for name, clf in classifiers:
    clf.fit(X_train, Y_train)
    train_accuracy = clf.score(X_train, Y_train)
    test_accuracy = clf.score(X_test, Y_test)
    # Append the accuracies to the dictionary
    accuracies[' '].append(name)
    accuracies['Training Accuracy'].append(train_accuracy)
    accuracies['Test Accuracy'].append(test_accuracy)


# In[11]:


# Convert the dictionary to a DataFrame
accuracy_df = pd.DataFrame(accuracies)

accuracy_df_transposed = accuracy_df.set_index(' ').T 

# Display the DataFrame
accuracy_df_transposed


# In[ ]:




