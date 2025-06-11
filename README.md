Розрахунково-графічна робота: Шаблони Проектування

Варіант:
1.Creational pattern: Prototype (Прототип)
2.Structural pattern: Composite (Компонувальник)
3.Behavioral pattern: Mediator (Посередник)
4.Concurrency pattern: Reactor (Реактор)

Виконання роботи

1. Породжуючий шаблон: Прототип (Prototype)
Призначення та застосування
Прототип — це породжуючий шаблон проектування, що дозволяє копіювати об'єкти, не вдаючись у деталі їх реалізації. Він пропонує створити "прототип" об'єкта, а потім створювати нові об'єкти, просто копіюючи цей прототип.

Застосування:
Коли код не повинен залежати від класів об'єктів, що копіюються.
Коли потрібно уникнути створення підкласів лише для ініціалізації об'єктів певним чином. Шаблон дозволяє налаштувати прототип і створювати копії з потрібним станом.
Коли створення об'єкта є ресурсозатратним (наприклад, вимагає запитів до бази даних або складних обчислень), а у вас вже є схожий об'єкт. Клонування може бути значно швидшим.

UML-діаграма

Діаграма класів:
classDiagram
    class Client {
        +operation()
    }

    class Prototype {
        <<interface>>
        +Clone() IPrototype
    }

    class ConcretePrototype1 {
        - someField
        +ConcretePrototype1(someField)
        +Clone() IPrototype
    }

    class ConcretePrototype2 {
        - anotherField
        +ConcretePrototype2(anotherField)
        +Clone() IPrototype
    }

    Client --> Prototype
    Prototype <|.. ConcretePrototype1
    Prototype <|.. ConcretePrototype2

    Діаграма послідовності:
sequenceDiagram
    participant C as Client
    participant P1 as ConcretePrototype1
    participant P2 as P1.Clone()

    C->>P1: Clone()
    activate P1
    P1->>P2: new ConcretePrototype1(this)
    P1-->>C: повертає P2
    deactivate P1

Опис основних структурних елементів:
Prototype (IPrototype): Оголошує інтерфейс для клонування самого себе. Зазвичай містить один метод Clone().
ConcretePrototype (ConcretePrototype1, ConcretePrototype2): Реалізує операцію клонування. Крім копіювання даних вихідного об'єкта в клон, цей метод може також враховувати деякі нюанси копіювання, пов'язані, наприклад, з клонуванням пов'язаних об'єктів, розв'язанням рекурсивних залежностей тощо.
Client: Створює новий об'єкт, звертаючись до прототипу з проханням клонувати себе.

Джерело інформації: [Refactoring Guru - Prototype](https://refactoring.guru/design-patterns/prototype)

2. Структурний шаблон: Компонувальник (Composite)
Призначення та застосування
Компонувальник — це структурний шаблон проектування, що дозволяє згрупувати безліч об'єктів у деревоподібну структуру, а потім працювати з нею так, ніби це один об’єкт.

Застосування:
Коли потрібно представити ієрархію об'єктів "частина-ціле".
Коли клієнти повинні однаково трактувати як прості, так і складові об'єкти. Це дозволяє клієнтському коду не перевіряти, з яким типом об'єкта він працює (простим "листком" чи складним "контейнером").
Добре підходить для роботи з рекурсивними структурами, такими як дерева файлової системи, меню програми або структура організації.

UML-діаграма

Діаграма класів:
classDiagram
    class Client {
        +operation(Component)
    }
    class Component {
        <<abstract>>
        +Operation()
        +Add(Component)
        +Remove(Component)
        +GetChild(int)
    }
    class Leaf {
        +Operation()
    }
    class Composite {
        -List~Component~ children
        +Operation()
        +Add(Component)
        +Remove(Component)
        +GetChild(int)
    }

    Client --> Component
    Component <|-- Leaf
    Component <|-- Composite
    Composite o-- Component

Діаграма послідовності:
sequenceDiagram
    participant C as Client
    participant Root as Composite
    participant L1 as Leaf
    participant Sub as Composite
    participant L2 as Leaf

    C->>Root: Operation()
    activate Root
    Root->>L1: Operation()
    Root->>Sub: Operation()
    activate Sub
    Sub->>L2: Operation()
    deactivate Sub
    deactivate Root

Опис основних структурних елементів:
Component (Component): Оголошує загальний інтерфейс як для простих (Leaf), так і для складових (Composite) об'єктів у структурі. Може надавати реалізацію за замовчуванням для методів додавання/видалення дочірніх елементів.
Leaf (Leaf): Представляє кінцевий вузол ("листок") дерева, який не може мати дочірніх елементів. Визначає поведінку для примітивних об'єктів у композиції.
Composite (Composite): Представляє складовий вузол ("контейнер"), який може мати дочірні елементи (листки або інші контейнери). Реалізує операції, визначені в інтерфейсі Component, зазвичай делегуючи їх виконання своїм дочірнім елементам.
Client: Працює з усіма об'єктами в структурі через інтерфейс Component.
Джерело інформації: [Refactoring Guru - Composite](https://refactoring.guru/design-patterns/composite)

3. Поведінковий шаблон: Посередник (Mediator)
Призначення та застосування
Посередник — це поведінковий шаблон проектування, який дозволяє зменшити зв'язність безлічі класів між собою, завдяки переміщенню цих зв'язків в один клас-посередник. Замість того, щоб компоненти взаємодіяли один з одним напряму, вони спілкуються через об'єкт-посередник.

Застосування:
Коли є багато компонентів, що тісно пов'язані між собою, і їх важко змінювати через наявність численних зв'язків.
Коли логіку взаємодії між компонентами складно зрозуміти та підтримувати.
Коли ви хочете перевикористовувати компоненти, але вони залежать від багатьох інших компонентів, що ускладнює їх ізоляцію.

UML-діаграма

Діаграма класів:
classDiagram
    class Mediator {
        <<interface>>
        +notify(sender, event)
    }
    class ConcreteMediator {
        -component1
        -component2
        +notify(sender, event)
    }
    class Colleague {
        <<abstract>>
        #mediator
        +Colleague(mediator)
    }
    class ConcreteColleague1 {
        +operationA()
    }
    class ConcreteColleague2 {
        +operationB()
    }

    Mediator <|.. ConcreteMediator
    Colleague <|-- ConcreteColleague1
    Colleague <|-- ConcreteColleague2
    ConcreteMediator o-- ConcreteColleague1
    ConcreteMediator o-- ConcreteColleague2
    ConcreteColleague1 --> Mediator
    ConcreteColleague2 --> Mediator

Діаграма послідовності:
sequenceDiagram
    participant C1 as ConcreteColleague1
    participant C2 as ConcreteColleague2
    participant M as ConcreteMediator

    C1->>M: notify(this, "EventA")
    activate M
    M->>C2: operationB()
    deactivate M

Опис основних структурних елементів:
Mediator (Mediator): Оголошує інтерфейс для спілкування з об'єктами Colleague.
ConcreteMediator (ConcreteMediator): Реалізує логіку взаємодії між об'єктами Colleague. Він знає про всіх колег і координує їхню роботу.
Colleague (Colleague): Оголошує інтерфейс для "колег" (компонентів), які спілкуються через посередника. Кожен колега має посилання на об'єкт-посередник.
ConcreteColleague (ConcreteColleague1, ConcreteColleague2): Конкретні класи компонентів. Кожен з них інформує посередника про зміни у своєму стані, а також реагує на запити від посередника.
Джерело інформації: [Refactoring Guru - Mediator](https://refactoring.guru/design-patterns/mediator)

4. Шаблон паралельних обчислень: Реактор (Reactor)
Призначення та застосування
Реактор (Reactor) — це шаблон проектування для паралельної обробки подій, який дозволяє обробляти запити від кількох клієнтів в одному потоці. Основна ідея полягає в тому, щоб реагувати на події введення-виведення, які надходять від різних джерел, і диспетчеризувати їх відповідним обробникам.
Цей шаблон дозволяє уникнути створення окремого потоку для кожного клієнта, що є неефективним при великій кількості з'єднань.

Застосування:
У серверних додатках, які повинні обробляти велику кількість одночасних клієнтських з'єднань (наприклад, веб-сервери, чат-сервери, проксі).
У системах, де потрібно ефективно керувати асинхронними операціями введення-виведення без блокування основного потоку.
Часто використовується в мережевих бібліотеках та фреймворках (наприклад, Node.js, Netty, Twisted).

UML-діаграма

Діаграма класів:
classDiagram
    class Reactor {
        -demultiplexer
        +registerHandler(handler, eventType)
        +removeHandler(handler)
        +handleEvents()
    }
    class EventHandler {
        <<interface>>
        +getHandle()
        +handleEvent(eventType)
    }
    class ConcreteEventHandlerA {
        +getHandle()
        +handleEvent(eventType)
    }
    class ConcreteEventHandlerB {
        +getHandle()
        +handleEvent(eventType)
    }
    class SynchronousEventDemultiplexer {
        +select(handlers)
    }
    class Handle {
        <<represents resource>>
    }

    Reactor o-- SynchronousEventDemultiplexer
    Reactor --> EventHandler : registers/removes
    EventHandler <|.. ConcreteEventHandlerA
    EventHandler <|.. ConcreteEventHandlerB
    EventHandler o-- Handle

Діаграма послідовності:
sequenceDiagram
    participant App as Application
    participant R as Reactor
    participant Demux as SynchronousEventDemultiplexer
    participant EH as EventHandler

    App->>R: registerHandler(EH, READ_EVENT)
    loop Event Loop
        R->>Demux: select()
        activate Demux
        Note over Demux: Чекає на подію I/O...
        Demux-->>R: повертає список активних Handle
        deactivate Demux
        R->>EH: handleEvent(READ_EVENT)
        activate EH
        Note over EH: Обробка даних (наприклад, читання з сокета)
        deactivate EH
    end

Опис основних структурних елементів:
Handles: Дескриптори (ідентифікатори) ресурсів, що надаються операційною системою для представлення джерел подій (наприклад, сокети, файли).
Synchronous Event Demultiplexer: Механізм, що ефективно очікує на події, які відбуваються на множині Handles. Він блокується доти, доки не станеться подія, після чого повертає список Handles, готових до обробки. (Приклади: select(), poll(), epoll() в Unix).
EventHandler: Інтерфейс, який визначає метод для обробки подій (handleEvent).
ConcreteEventHandler: Конкретна реалізація обробника подій, що містить логіку для обробки певного типу подій (наприклад, читання даних з сокета). Кожен обробник пов'язаний з певним Handle.
Reactor: Основний компонент, що керує циклом обробки подій. Він реєструє/видаляє обробники, використовує Demultiplexer для очікування подій і викликає відповідний handleEvent у зареєстрованому обробнику, коли надходить подія.
Джерело інформації: [Pattern-Oriented Software Architecture, Volume 2](https://www.researchgate.net/publication/215835789_Pattern-Oriented_Software_Architecture_Patterns_for_Concurrent_and_Networked_Objects_Volume_2)
