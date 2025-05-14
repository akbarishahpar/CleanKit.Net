using CleanKit.Net.Persistence.DatabaseContext;

namespace CleanKit.Net.Template.Persistence.DatabaseContexts;

public class MainDbContextFactory()
    : DatabaseContextFactory<MainDbContext>("Data Source=.\\SQLExpress;" // TODO: SQL INSTANCE NAME GOES HERE
                                            + "Initial Catalog=CleanKit.Net.TemplateDb;" // TODO: DEV DATABASE NAME GOES HERE
                                            + "Persist Security Info=True;"
                                            + "Integrated Security=SSPI;"
                                            + "TrustServerCertificate=True");