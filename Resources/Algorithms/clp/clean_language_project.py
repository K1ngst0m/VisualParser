#  ██████╗██╗     ██████╗
# ██╔════╝██║     ██╔══██╗
# ██║     ██║     ██████╔╝
# ██║     ██║     ██╔═══╝
# ╚██████╗███████╗██║
#  ╚═════╝╚══════╝╚═╝

########## 语言模块 ##########
# - 功能:
#  1) 转发数据
#  2) 解析字库数据并处理

# coding = utf-8
import os
import sys
import re


def read_func(str_file_path):
    temp_words_list = []
    with open(str_file_path, 'r', encoding="utf8") as file:
        for line in file.readlines():
            liner = line.strip()
            temp_words_list.append(liner)
    return temp_words_list


class clean_language:
    def __init__(self):
        # 这两个库是出现直接ban的
        self.__sensitive_words_list = read_func('word_lists//sensitive_words.utf8')
        self.__dirty_list = read_func('word_lists//dirty_words.utf8')

        # 需要判断的库
        self.__subject_good_list = read_func('word_lists//subject_good.utf8')
        self.__subject_bad_list = read_func('word_lists//subject_bad.utf8')
        self.__good_adj_list = read_func('word_lists//good_adj.utf8')
        self.__bad_adj_list = read_func('word_lists//bad_adj.utf8')
        self.__judge_no_list = read_func('word_lists//judge_no.utf8')

    # def main_controler_list_input(self,list_input):

    def clean_func(self, scr_input):
        # print("测试内容: " + scr_input)
        pattern = r',|\.|/|;|\'|`|\[|\]|<|>|\?|:|"|\{|\}|\~|!|@|#|\$|%|\^|&|\(|\)|-|=|\_|\+|，|。|、|；|‘|’|【|】|·|！| |…|（|）'
        split_text_list = re.split(pattern, scr_input)  # 使用DFA算法比较快

        for t in range(len(split_text_list)):
            test_input = ' ' + split_text_list[t]  # 句子第一个要空格不然会有bug

            word_flag = 0

            # 直接ban的部分
            if word_flag == 0:
                for i in range(len(self.__sensitive_words_list)):  # 单个ban词，有的话不显示
                    temp_word = self.__sensitive_words_list[i]
                    if test_input.find(temp_word) > 0:
                        print("检索路径: adult_words_list")
                        # print(temp_word)
                        word_flag = 1
                        break

            if word_flag == 0:
                for i in range(len(self.__dirty_list)):
                    temp_word = self.__dirty_list[i]
                    if test_input.find(temp_word) > 0:
                        print("检索路径: dirty_list")
                        # print(temp_word)
                        word_flag = 1
                        break

            # 主体加形容词判断

            if word_flag == 0:
                for j in range(len(self.__subject_good_list)):  # 好主体
                    if word_flag > 0:
                        break

                    if test_input.find(self.__subject_good_list[j]) > 0:  # 搜主体

                        for i in range(len(self.__bad_adj_list)):
                            if test_input.find(self.__bad_adj_list[i]) > 0 and word_flag == 0:  # 搜不好的形容词
                                count_deny = 0  # 否定计数器
                                search_ptr = 0  # 搜索指针
                                jump_flag = 0
                                while True:
                                    if search_ptr >= len(test_input):
                                        break
                                    if jump_flag == 1:
                                        break
                                    for k in range(len(self.__judge_no_list)):
                                        test_num = test_input.find(self.__judge_no_list[k])
                                        if test_input.find(self.__judge_no_list[k],
                                                           search_ptr) == -1:  # 搜判断词,是否检测到否定，以及判断是否为双重否定
                                            pass  # 未检测到否定
                                            jump_flag = 1
                                        else:
                                            count_deny = count_deny + 1  # 检测到否定
                                            search_ptr = test_input.find(self.__judge_no_list[k],
                                                                         search_ptr) + 1  # 计数器移位
                                if count_deny % 2 == 0:  # 判断是否双重否定
                                    word_flag = 1  # 无否定,属于GS-Y-BA
                                else:
                                    word_flag = 0  # 否定,属于GS-N-BA

                        for i in range(len(self.__good_adj_list)):
                            if test_input.find(self.__good_adj_list[i]) > 0 and word_flag == 0:  # 搜好的形容词
                                count_deny = 0  # 否定计数器
                                search_ptr = 0  # 搜索指针
                                jump_flag = 0
                                while True:
                                    if search_ptr >= len(test_input):
                                        break
                                    if jump_flag == 1:
                                        break
                                    for k in range(len(self.__judge_no_list)):
                                        test_num = test_input.find(self.__judge_no_list[k])
                                        if test_input.find(self.__judge_no_list[k],
                                                           search_ptr) == -1:  # 搜判断词,是否检测到否定，以及判断是否为双重否定
                                            pass  # 未检测到否定
                                            jump_flag = 1
                                        else:
                                            count_deny = count_deny + 1  # 检测到否定
                                            search_ptr = test_input.find(self.__judge_no_list[k],
                                                                         search_ptr) + 1  # 计数器移位

                                if count_deny % 2 == 0:  # 判断是否双重否定
                                    word_flag = 0  # 无否定,属于GS-Y-GA
                                else:
                                    word_flag = 1  # 否定,属于GS-N-GA

                if word_flag == 1:
                    print("检索路径: 形容词判断")

            if word_flag == 0:
                for j in range(len(self.__subject_bad_list)):  # 坏主体
                    if word_flag > 0:
                        break

                    if test_input.find(self.__subject_bad_list[j]) > 0:  # 搜主体

                        for i in range(len(self.__bad_adj_list)):
                            if test_input.find(self.__bad_adj_list[i]) > 0 and word_flag == 0:  # 搜不好的形容词
                                count_deny = 0  # 否定计数器
                                search_ptr = 0  # 搜索指针
                                jump_flag = 0
                                while True:
                                    if search_ptr >= len(test_input):
                                        break
                                    if jump_flag == 1:
                                        break
                                    for k in range(len(self.__judge_no_list)):
                                        test_num = test_input.find(self.__judge_no_list[k])
                                        if test_input.find(self.__judge_no_list[k],
                                                           search_ptr) == -1:  # 搜判断词,是否检测到否定，以及判断是否为双重否定
                                            pass  # 未检测到否定
                                            jump_flag = 1
                                        else:
                                            count_deny = count_deny + 1  # 检测到否定
                                            search_ptr = test_input.find(self.__judge_no_list[k],
                                                                         search_ptr) + 1  # 计数器移位
                                if count_deny % 2 == 0:  # 判断是否双重否定
                                    word_flag = 0  # 无否定,属于BS-Y-BA
                                else:
                                    word_flag = 1  # 否定,属于BS-N-BA

                        for i in range(len(self.__good_adj_list)):
                            if test_input.find(self.__good_adj_list[i]) > 0 and word_flag == 0:  # 搜好的形容词
                                count_deny = 0  # 否定计数器
                                search_ptr = 0  # 搜索指针
                                jump_flag = 0
                                while True:
                                    if search_ptr >= len(test_input):
                                        break
                                    if jump_flag == 1:
                                        break
                                    for k in range(len(self.__judge_no_list)):
                                        test_num = test_input.find(self.__judge_no_list[k])
                                        if test_input.find(self.__judge_no_list[k],
                                                           search_ptr) == -1:  # 搜判断词,是否检测到否定，以及判断是否为双重否定
                                            pass  # 未检测到否定
                                            jump_flag = 1
                                        else:
                                            count_deny = count_deny + 1  # 检测到否定
                                            search_ptr = test_input.find(self.__judge_no_list[k],
                                                                         search_ptr) + 1  # 计数器移位

                                if count_deny % 2 == 0:  # 判断是否双重否定
                                    word_flag = 1  # 无否定,属于BS-Y-GA
                                else:
                                    word_flag = 0  # 否定,属于BS-N-GA
            # print(word_flag)

            if word_flag > 0:
                print("测试结果: 判断为敏感信息")
                break
        if word_flag == 0:
            print("测试结果: 内容通过")

        return word_flag


if __name__ == '__main__':
    os.chdir(os.path.split(os.path.realpath(__file__))[0])
    cleaner = clean_language()
    sys.argv.remove(sys.argv[0])
    for arg in sys.argv:
        print("#A#---------------------------------\n")
        cleaner.clean_func(arg)
        print("\n---------------------------------#B#")
    sys.exit()
