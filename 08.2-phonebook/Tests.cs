namespace task_08_2_tests;

public struct Tests
{
    public static readonly string[] CmdSeq = {"/add +7 923 4-333-007 Levchuk Rinat Vladimirovich",
                                              "/add +7 111 1-111-111 Ivanov Ivan Ivanovich",
                                              "/add +7 222 2-222-222 Petrov Ivan Ivanovich",
                                              "/ls",
                                              "/add +7 333 3-333-333 Sidorov Ivan Ivanovich",
                                              "/rm +7 923 4-333-007",
                                              "/ls",
                                              "/find +7 111 1-111-111",
                                              "/exit"};
}