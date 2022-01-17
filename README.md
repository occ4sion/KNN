# KNN
### Метрический классификатор на C#

> Модель распознаёт сложные паттерны в пользовательских рисунках и раскрашивает точки в необходимые цвета.

Тёмными оттенками помечены закрашенные моделью точки.

<img src="https://github.com/occ4sion/KNN/blob/master/preview/preview.png" width="600" alt="Иллюстрация"/>

Реализация непараметрического метода Парзеновского окна с фиксированной шириной различными типами ядер над метрикой минковского для пользовательских объектов в двумерном евклидовом пространстве.

#### Как попробовать?
1. Скачайте все файлы из `bin/Debug/net6.0-windows`.
2. Установите `.NET Descktop Runtime`.
3. Запустите `KNN.exe`.
4. Когда закончите рисовать, `обучите` модель.
5. Поставьте точки для раскрашивания и нажмите `Предсказать`.

|version 1.0|version 2.2|
|-----------|-----------|
| <img src="https://github.com/occ4sion/KNN/blob/master/preview/KNN1.gif" width="400" alt="v1.0"/> | <img src="https://github.com/occ4sion/KNN/blob/master/preview/KNN2.gif" width="400" alt="v2.2"/> |

#### TODO:

- [ ] Multiclass
- [ ] More kernels
- [ ] Add parameters
- [x] Fill canvas