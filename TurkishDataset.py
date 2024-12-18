import pandas as pd
import numpy as np

from sklearn.model_selection import train_test_split
from tensorflow.keras.layers import Dense, Dropout, Embedding, LSTM
from tensorflow.keras.models import Sequential
from tensorflow.keras.preprocessing.sequence import pad_sequences
from tensorflow.keras.preprocessing.text import Tokenizer
from sklearn.metrics import accuracy_score, precision_score, recall_score, f1_score, classification_report, confusion_matrix

dataset = pd.read_csv('datasets/Dataset.csv')
dataset = dataset.set_index('id', drop = True)

print(dataset.shape)
dataset.head()

print('missing values counts\n', dataset.isnull().sum())

length = []
[length.append(len(str(text))) for text in dataset['text']]
dataset['length'] = length
print('data length\n', dataset.head())

print('min data length', min(dataset['length']), ', max data length', max(dataset['length']), ', average data length', round(sum(dataset['length'])/len(dataset['length'])))

print('count of less then 50 character', len(dataset[dataset['length'] < 50]))

# dropping the outliers
dataset = dataset.drop(dataset['text'][dataset['length'] < 50].index, axis = 0)
print('min data length', min(dataset['length']), ', max data length', max(dataset['length']), ', average data length', round(sum(dataset['length'])/len(dataset['length'])))
print(dataset.shape)

max_features = 2500

# Tokenizing the text - converting the words, letters into counts or numbers.
# We dont need to explicitly remove the punctuations. we have an inbuilt option in Tokenizer for this purpose
tokenizer = Tokenizer(num_words = max_features, filters='!"#$%&()*+,-./:;<=>?@[\\]^_`{|}~\t\n', lower = True, split = ' ')
tokenizer.fit_on_texts(texts = dataset['text'])
X = tokenizer.texts_to_sequences(texts = dataset['text'])

# now applying padding to make them even shaped.
X = pad_sequences(sequences = X, maxlen = max_features, padding = 'pre')

print('X shape', X.shape)
y = dataset['label'].values
print('Y shape', y.shape)

# splitting the data training data for training and validation.
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size = 0.2, random_state = 101)

# LSTM Neural Network
lstm_model = Sequential(name = 'lstm_nn_model')
lstm_model.add(layer = Embedding(input_dim = max_features, output_dim = 120, name = '1st_layer'))
lstm_model.add(layer = LSTM(units = 120, dropout = 0.2, recurrent_dropout = 0.2, name = '2nd_layer'))
lstm_model.add(layer = Dropout(rate = 0.5, name = '3rd_layer'))
lstm_model.add(layer = Dense(units = 120,  activation = 'relu', name = '4th_layer'))
lstm_model.add(layer = Dropout(rate = 0.5, name = '5th_layer'))
lstm_model.add(layer = Dense(units = len(set(y)),  activation = 'sigmoid', name = 'output_layer'))
# compiling the model
lstm_model.compile(optimizer = 'adam', loss = 'sparse_categorical_crossentropy', metrics = ['accuracy'])

lstm_model_fit = lstm_model.fit(X_train, y_train, epochs = 10)

lstm_prediction = lstm_model.predict(X_test)
lstm_prediction_vec = np.argmax(lstm_prediction, axis=1)

print("lstm_prediction", lstm_prediction_vec)

accuracy = accuracy_score(y_test, lstm_prediction_vec)
precision = precision_score(y_test, lstm_prediction_vec, average='weighted')
recall = recall_score(y_test, lstm_prediction_vec, average='weighted')
f1 = f1_score(y_test, lstm_prediction_vec, average='weighted')
confisiun_matrix = confusion_matrix(y_test, lstm_prediction_vec)

classification_rep = classification_report(y_test, lstm_prediction_vec,)

print(f"Accuracy: {accuracy:.2f}")
print(f"Precision: {precision:.2f}")
print(f"Recall: {recall:.2f}")
print(f"F1-Score: {f1:.2f}")
print(f"F1-Score: {f1:.2f}")
print(f"Classification Report:\n {classification_rep}")
print(f"Confisiun Matrix:\n {confisiun_matrix}")