import argparse


def prRed(skk): print("\033[91m{}\033[00m" .format(skk))


def prGreen(skk): print("\033[92m{}\033[00m" .format(skk))


def prYellow(skk): print("\033[93m{}\033[00m" .format(skk))


def prLightPurple(skk): print("\033[94m{}\033[00m" .format(skk))


def prPurple(skk): print("\033[95m{}\033[00m" .format(skk))


def prCyan(skk): print("\033[96m{}\033[00m" .format(skk))


def prLightGray(skk): print("\033[97m{}\033[00m" .format(skk))


def prBlack(skk): print("\033[98m{}\033[00m" .format(skk))


prCyan(".NET helper Developed By Arijit Kundu")


def getRepoInterfaceCodeAndName(entity):
    ic = "using CTS_BE.DAL.Entities;\n"\
        "namespace CTS_BE.DAL.Interfaces\n"\
        "{\n"\
        "    public interface I||ENTITY||Repository: IRepository<||ENTITY||>\n"\
        "    {\n"\
        "    }\n"\
        "}"\
        ""
    ic = ic.replace("||ENTITY||", entity)
    name = f"I{entity}Repository.cs"
    return [name, ic]


def getRepoCodeAndName(entity):
    rc = "using CTS_BE.DAL.Entities;\n"\
        "using CTS_BE.DAL.Interfaces;\n"\
        "namespace CTS_BE.DAL\n"\
        "{\n"\
        "   public class ||ENTITY||Repository : Repository<||ENTITY||, CTSDBContext>, I||ENTITY||Repository\n"\
        "   {\n"\
        "       public ||ENTITY||Repository(CTSDBContext context) : base(context)\n"\
        "       {\n"\
        "       }\n"\
        "   }\n"\
        "}"\
        ""
    rc = rc.replace("||ENTITY||", entity)
    name = f"{entity}Repository.cs"
    return [name, rc]


def getServcInterfaceCodeAndName(entity):
    ic = "using CTS_BE.Models;\n"\
        "namespace CTS_BE.BAL.Interfaces\n"\
        "{\n"\
        "    public interface I||ENTITY||Service\n"\
        "    {\n"\
        "    }\n"\
        "}"\
        ""
    ic = ic.replace("||ENTITY||", entity)
    name = f"I{entity}Service.cs"
    return [name, ic]


def getServcCodeAndName(entity):
    sc = "using AutoMapper;\n"\
        "using CTS_BE.BAL.Interfaces;\n"\
        "using CTS_BE.DAL.Entities;\n"\
        "using CTS_BE.DAL.Interfaces;\n"\
        "using CTS_BE.Models;\n"\
        "namespace CTS_BE.BAL\n"\
        "{\n"\
        "    public class ||ENTITY||Service : I||ENTITY||Service\n"\
        "    {\n"\
        "        private readonly I||ENTITY||Repository _||ENTITY||Repository;\n"\
        "        private readonly IMapper _mapper;\n"\
        "        public ||ENTITY||Service(I||ENTITY||Repository ||ENTITY||Repository, IMapper mapper) {\n"\
        "            _||ENTITY||Repository = ||ENTITY||Repository;\n"\
        "            _mapper = mapper;\n"\
        "        }\n"\
        "    }\n"\
        "}"\
        ""
    sc = sc.replace("||ENTITY||", entity)
    name = f"{entity}Service.cs"
    return [name, sc]


def create_file(path, CnN):
    f = open(f"{path}{CnN[0]}", "w")
    f.write(CnN[1])
    prGreen(f"[+] {path}{CnN[0]} successfully created")
    f.close()


parser = argparse.ArgumentParser(
    prog='ark.exe',
    usage=None,
    description='[+] ::: .NET helper By ARK:::',
    # formatter_class= < class 'argparse.HelpFormatter' >,
    conflict_handler='error',
    add_help=True
)
parser.add_argument("entity")
parser.add_argument("-r", action="store_true")
parser.add_argument("-s", action="store_true")
parser.add_argument("-p", action="store_true")
parser.add_argument("-m", action="store_true")
args = parser.parse_args()

print('[+] Entity: ', end='')
prYellow(f"{args.entity}")

if args.r:
    repoICnN = getRepoInterfaceCodeAndName(args.entity)
    repoCnN = getRepoCodeAndName(args.entity)
    create_file("./DAL/Interfaces/", repoICnN)
    create_file("./DAL/Repositories/", repoCnN)

if args.s:
    servcICnN = getServcInterfaceCodeAndName(args.entity)
    servcCnN = getServcCodeAndName(args.entity)
    create_file("./BAL/Interfaces/", servcICnN)
    create_file("./BAL/Services/", servcCnN)

if args.p:
    f = open("Program.cs", "r")
    c = f.read()
    f.close()
    if args.r:
        c = c.replace(
            "//Repositories", f"//Repositories\nbuilder.Services.AddTransient<I{args.entity}Repository, {args.entity}Repository>();")
    if args.s:
        c = c.replace(
            "//Services", f"//Services\nbuilder.Services.AddTransient<I{args.entity}Service, {args.entity}Service>();")
    f = open("Program.cs", "w")
    f.write(c)
    prYellow("[+] Inserting mapping in Program.cs")
    f.close()

if args.m:
    model_code = []
    f = open(f"./DAL/Entities/{args.entity}.cs", "r")
    lines = f.readlines()
    sf = False
    for line in lines:
        if line[0:6] == "public":
            sf = True
            line = line.replace(" partial","").replace("\n","") + "Model {\n"
        if line.strip()[0:6] == "public":
            model_code.append("    "+line)
    f.close()
    f = open(f"./Models/{args.entity}Model.cs", "w")
    model_code.append("    }\n")
    model_code.append("}")
    f.writelines(["namespace CTS_BE.Models{\n"])
    f.writelines(model_code)
    f.close()
    prGreen(f"[+] ./Models/{args.entity}Model.cs successfully created")
